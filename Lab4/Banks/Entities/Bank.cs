using Banks.Accounts;
using Banks.Accounts.AccountConfigurations;
using Banks.Accounts.Commands;
using Banks.Accounts.Factory;
using Banks.Builders;
using Banks.Interfaces;
using Banks.Models;
using Banks.Observer;
using Banks.Service;
using Banks.Transaction;

namespace Banks.Entities;

public class Bank : IObservable
{
    private readonly CentralBank _centralBank = new CentralBank();
    private readonly List<BaseAccount> _bankAccounts = new ();
    private readonly List<IObserver> _subscribers = new ();
    private readonly BankConfiguration _bankConfiguration;

    public Bank(string name, double debitPercent, Dictionary<Range, Percent> depositPercents, double creditCommission, decimal creditLimit, decimal limitForDubiousClient, uint depositPeriodInDays)
    {
        ArgumentNullException.ThrowIfNull(nameof(name));
        Name = name;
        Id = Guid.NewGuid();

        _bankConfiguration = new ConfigurationBuilder()
            .AddCommission(new Commission(creditCommission))
            .AddCreditLimit(new Limit(creditLimit))
            .AddDebitPercent(new Percent(debitPercent))
            .AddDepositPercent(depositPercents)
            .AddLimitForDubiousClient(new Limit(limitForDubiousClient))
            .AddDepositPeriodInDays(depositPeriodInDays)
            .Build();
    }

    public Guid Id { get; }
    public string Name { get; }

    public BaseAccount? FindAccount(Guid accountId) => _bankAccounts.SingleOrDefault(x => x.Id.Equals(accountId));

    public BaseAccount GetAccount(Guid accountId) => FindAccount(accountId) ?? throw new Exception();

    public void Subscribe(IObserver observer)
    {
        ArgumentNullException.ThrowIfNull(nameof(observer));
        if (_subscribers.Contains(observer))
        {
            throw new Exception();
        }

        _subscribers.Add(observer);
    }

    public void Unsubscribe(IObserver observer)
    {
        ArgumentNullException.ThrowIfNull(nameof(observer));
        if (!_subscribers.Remove(observer))
        {
            throw new Exception();
        }
    }

    public void Notify()
    {
        _subscribers.ForEach(x => x.Update());
    }

    internal BaseAccount CreateAccount(TypeOfBankAccount typeOfBankAccount, IClient client, decimal amount, uint? depositPeriodInDays = null)
    {
        return typeOfBankAccount switch
        {
            TypeOfBankAccount.Credit => AccountFactory.CreateCreditAccount(client, amount, _bankConfiguration),
            TypeOfBankAccount.Debit => AccountFactory.CreateDebitAccount(client, amount, _bankConfiguration),
            TypeOfBankAccount.Deposit=> AccountFactory.CreateDepositAccount(client, amount, _bankConfiguration, depositPeriodInDays),
            _ => throw new ArgumentOutOfRangeException(nameof(typeOfBankAccount), typeOfBankAccount, null)
        };
    }

    internal BankTransaction Income(Guid accountId, decimal sum)
    {
        BaseAccount account = _bankAccounts.FirstOrDefault(x => x.Id.Equals(accountId)) ?? throw new Exception();
        var transaction = new BaseTransaction(new Income(account, sum));
        transaction.DoTransaction();
        account.SaveChanges(transaction);
        return transaction;
    }

    internal BankTransaction Withdraw(Guid accountId, decimal sum)
    {
        BaseAccount account = _bankAccounts.FirstOrDefault(x => x.Id.Equals(accountId)) ?? throw new Exception();
        var transaction = new BaseTransaction(new Withdraw(account, sum));
        transaction.DoTransaction();
        account.SaveChanges(transaction);
        return transaction;
    }
}