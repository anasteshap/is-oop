using Banks.Accounts;
using Banks.Accounts.AccountConfigurations;
using Banks.Accounts.Commands;
using Banks.Builders;
using Banks.DateTimeProvider;
using Banks.Entities;
using Banks.Exceptions;
using Banks.Interfaces;
using Banks.Models;
using Banks.Transaction;

namespace Banks.Service;

public class CentralBank : ICentralBank
{
    private readonly List<Bank> _banks = new ();
    private readonly List<Client> _clients = new ();

    public CentralBank(RewindClock rewindClock)
    {
        ArgumentNullException.ThrowIfNull(nameof(rewindClock));
        RewindClock = rewindClock;
    }

    public RewindClock RewindClock { get; }

    public IClient RegisterClient(string name, string surname, string? address = null, long? passport = null)
    {
        Client client = new ClientBuilder()
            .AddName(name)
            .AddSurname(surname)
            .AddAddress(address)
            .AddPassportNumber(passport)
            .Build();
        _clients.Add(client);
        return client;
    }

    public Bank? FindBankByName(string name) => _banks.FirstOrDefault(x => x.Name.Equals(name));

    public Bank GetBankByName(string name) => FindBankByName(name) ?? throw BankException.BankDoesNotExist(name);
    public IClient? FindClientById(Guid id) => _clients.FirstOrDefault(x => x.Id.ToString().Equals(id.ToString()));

    public IClient GetClientById(Guid id) => FindClientById(id) ?? throw ClientException.ClientDoesNotExist(id);
    public IReadOnlyCollection<IClient> GetAlClients() => _clients;

    public IReadOnlyCollection<Bank> GetAllBanks() => _banks;

    public Bank CreateBank(string name, decimal debitPercent, List<DepositPercent> depositPercents, decimal creditCommission, decimal creditLimit, decimal limitForDubiousClient, TimeSpan endOfPeriod)
    {
        if (_banks.Exists(x => x.Name.Equals(name)))
            throw BankException.BankAlreadyExists(name);

        var credit = new CreditAccountConfiguration(creditCommission, creditLimit);
        var debit = new DebitAccountConfiguration(new Percent(debitPercent));
        var deposit = new DepositAccountConfiguration(depositPercents, endOfPeriod);
        var bankConfiguration = new BankConfiguration(credit, debit, deposit, new Limit(limitForDubiousClient));

        var bank = new Bank(name, RewindClock, bankConfiguration);
        _banks.Add(bank);
        return bank;
    }

    public BankTransaction ReplenishAccount(Guid bankId, Guid accountId, decimal amount)
    {
        Bank bank = _banks.SingleOrDefault(x => x.Id.Equals(bankId))
                    ?? throw BankException.BankDoesNotExist($"upd: with Id: {bankId.ToString()}");
        return bank.Income(accountId, amount);
    }

    public BankTransaction WithdrawMoney(Guid bankId, Guid accountId, decimal amount)
    {
        Bank bank = _banks.SingleOrDefault(x => x.Id.Equals(bankId))
                    ?? throw BankException.BankDoesNotExist($"upd: with Id: {bankId.ToString()}");
        return bank.Withdraw(accountId, amount);
    }

    public BankTransaction TransferMoney(Guid bankId1, Guid accountId1, Guid bankId2, Guid accountId2, decimal amount)
    {
        Bank bank1 = _banks.SingleOrDefault(x => x.Id.Equals(bankId1))
                     ?? throw BankException.BankDoesNotExist($"upd: with Id: {bankId1.ToString()}");
        Bank bank2 = _banks.SingleOrDefault(x => x.Id.Equals(bankId2))
                     ?? throw BankException.BankDoesNotExist($"upd: with Id: {bankId2.ToString()}");
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
        Bank bank = _banks.SingleOrDefault(x => x.Id.Equals(bankId))
                    ?? throw BankException.BankDoesNotExist($"upd: with Id: {bankId.ToString()}");
        bank.GetAccount(accountId).GetTransaction(transactionId).Undo();
    }
}