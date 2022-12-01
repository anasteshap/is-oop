using Banks.Accounts;
using Banks.Accounts.AccountConfigurations;
using Banks.Accounts.Commands;
using Banks.Builders;
using Banks.Entities;
using Banks.Interfaces;
using Banks.Models;
using Banks.Transaction;

namespace Banks.Service;

public class CentralBank : ICentralBank
{
    private readonly List<Bank> _banks = new ();
    private readonly List<Client> _clients = new ();

    public IClient RegisterClient(string name, string surname, string? address = null, long? passport = null)
    {
        Client client = new ClientBuilder().AddName(name).AddSurname(surname).AddAddress(address)
            .AddPassportNumber(passport).Build();
        _clients.Add(client);
        return client;
    }

    public void AddClientInfo(IClient client, string? address = null, long passport = default)
    {
        if (!_clients.Contains(client))
        {
            throw new Exception($"Клиента с именем {client.Surname} {client.Name} нет");
        }

        if (address is not null)
        {
            client.SetAddress(address);
        }

        if (passport != default)
        {
            client.SetPassportNumber(passport);
        }
    }

    public Bank? FindBankByName(string name) => _banks.FirstOrDefault(x => x.Name == name);

    public Bank GetBankByName(string name) => FindBankByName(name) ?? throw new Exception($"Банка с именем {name} нет");

    public BaseAccount? FindAccountById(string bankName, Guid accountId)
    {
        Bank bank = GetBankByName(bankName);
        return bank.FindAccount(accountId);
    }

    public IReadOnlyCollection<Bank> GetAllBanks() => _banks;

    public BankConfiguration CreateConfiguration(double debitPercent, Dictionary<Range, Percent> depositPercents, double creditCommission, decimal creditLimit, decimal limitForDubiousClient, uint depositPeriodInDays)
    {
        return new ConfigurationBuilder()
            .AddCommission(new Commission(creditCommission))
            .AddCreditLimit(new Limit(creditLimit))
            .AddDebitPercent(new Percent(debitPercent))
            .AddDepositPercent(depositPercents)
            .AddLimitForDubiousClient(new Limit(limitForDubiousClient))
            .AddDepositPeriodInDays(depositPeriodInDays)
            .Build();
    }

    public Bank CreateBank(string name, BankConfiguration bankConfiguration)
    {
        if (_banks.Exists(x => x.Name.Equals(name)))
        {
            throw new Exception();
        }

        var bank = new Bank(name, bankConfiguration);
        _banks.Add(bank);
        return bank;
    }

    public BaseAccount CreateCreditAccount(Bank bank, IClient client)
    {
        return bank.CreateAccount(TypeOfBankAccount.Credit, client);
    }

    public BaseAccount CreateDebitAccount(Bank bank, IClient client)
    {
        return bank.CreateAccount(TypeOfBankAccount.Debit, client);
    }

    public BaseAccount CreateDepositAccount(Bank bank, IClient client, uint? depositPeriodInDays = null)
    {
        return bank.CreateAccount(TypeOfBankAccount.Deposit, client, depositPeriodInDays);
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

        var transactionFrom = new ChainTransaction(new Withdraw(fromAccount, amount));
        var transactionTo = new ChainTransaction(new Income(toAccount, amount));
        transactionFrom.SetNext(transactionTo);

        transactionFrom.DoTransaction();
        fromAccount.SaveChanges(transactionFrom);

        transactionTo.DoTransaction();
        toAccount.SaveChanges(transactionTo);

        return transactionFrom;
    }

    public void CancelTransaction(Guid bankId, Guid accountId, Guid transactionId)
    {
        Bank bank = _banks.SingleOrDefault(x => x.Id.Equals(bankId)) ?? throw new Exception();
        bank.GetAccount(accountId).GetTransaction(transactionId).Undo();
    }

    public void ChangeDebitPercent(Guid bankId, double percent)
    {
        throw new NotImplementedException();
    }

    public void ChangeDepositPercent(Guid bankId, double percent)
    {
        throw new NotImplementedException();
    }

    public void ChangeCreditCommission(Guid bankId, double commissionPercent)
    {
        throw new NotImplementedException();
    }

    public void ChangeCreditLimit(Guid bankId, decimal creditLimit)
    {
        throw new NotImplementedException();
    }

    public void ChangeLimitForDubiousClient(Guid bankId, decimal limitForDubiousClient)
    {
        throw new NotImplementedException();
    }
}