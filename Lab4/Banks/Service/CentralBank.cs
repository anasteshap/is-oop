using Banks.Accounts;
using Banks.Accounts.AccountConfigurations;
using Banks.Accounts.Commands;
using Banks.Builders;
using Banks.DateTimeProvider;
using Banks.Entities;
using Banks.Interfaces;
using Banks.Models;
using Banks.Transaction;

namespace Banks.Service;

public class CentralBank : ICentralBank
{
    private readonly List<Bank> _banks = new ();
    private readonly List<Client> _clients = new (); // убрать // FindClientByName

    public CentralBank(RewindClock rewindClock)
    {
        RewindClock = rewindClock;
    }

    public RewindClock RewindClock { get; }

    public IClient RegisterClient(string name, string surname, string? address = null, long? passport = null)
    {
        Client client = new ClientBuilder().AddName(name).AddSurname(surname).AddAddress(address)
            .AddPassportNumber(passport).Build();
        _clients.Add(client);
        return client;
    }

    public Bank? FindBankByName(string name) => _banks.FirstOrDefault(x => x.Name == name);

    public Bank GetBankByName(string name) => FindBankByName(name) ?? throw new Exception($"Банка с именем {name} нет");

    public IReadOnlyCollection<Bank> GetAllBanks() => _banks;

    public void CreateBank(string name, decimal debitPercent, List<DepositPercent> depositPercents, decimal creditCommission, decimal creditLimit, decimal limitForDubiousClient, TimeSpan endOfPeriod)
    {
        if (_banks.Exists(x => x.Name.Equals(name)))
            throw new Exception();

        var credit = new CreditAccountConfiguration(creditCommission, creditLimit);
        var debit = new DebitAccountConfiguration(new Percent(debitPercent));
        var deposit = new DepositAccountConfiguration(depositPercents, endOfPeriod);
        var bankConfiguration = new BankConfiguration(credit, debit, deposit, new Limit(limitForDubiousClient));

        var bank = new Bank(name, RewindClock, bankConfiguration);
        _banks.Add(bank);
    }

    public BankTransaction ReplenishAccount(Guid bankId, Guid accountId, decimal amount)
    {
        Bank bank = _banks.SingleOrDefault(x => x.Id.Equals(bankId)) ?? throw new Exception();
        return bank.Income(accountId, amount);
    }

    public BankTransaction WithdrawMoney(Guid bankId, Guid accountId, decimal amount)
    {
        Bank bank = _banks.SingleOrDefault(x => x.Id.Equals(bankId)) ?? throw new Exception();
        return bank.Withdraw(accountId, amount);
    }

    public BankTransaction TransferMoney(Guid bankId1, Guid accountId1, Guid bankId2, Guid accountId2, decimal amount)
    {
        Bank bank1 = _banks.SingleOrDefault(x => x.Id.Equals(bankId1)) ?? throw new Exception();
        Bank bank2 = _banks.SingleOrDefault(x => x.Id.Equals(bankId2)) ?? throw new Exception();
        BaseAccount fromAccount = bank1.GetAccount(accountId1);
        BaseAccount toAccount = bank2.GetAccount(accountId2);

        var transaction = new BaseTransaction(new Transfer(toAccount, fromAccount, amount));
        transaction.DoTransaction();
        toAccount.SaveChanges(transaction);
        fromAccount.SaveChanges(transaction);

        return transaction;
    }

    public void CancelTransaction(Guid bankId, Guid accountId, Guid transactionId)
    {
        Bank bank = _banks.SingleOrDefault(x => x.Id.Equals(bankId)) ?? throw new Exception();
        bank.GetAccount(accountId).GetTransaction(transactionId).Undo();
    }
}