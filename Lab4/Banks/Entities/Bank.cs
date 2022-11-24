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

    public Bank(string name, double debitPercent, double depositPercent, double creditCommission, decimal creditLimit, decimal limitForDubiousClient)
    {
        ArgumentNullException.ThrowIfNull(nameof(name));
        Name = name;
        Id = Guid.NewGuid();

        _bankConfiguration = new ConfigurationBuilder()
            .AddCommission(new Commission(creditCommission))
            .AddCreditLimit(new Limit(creditLimit))
            .AddDebitPercent(new Percent(debitPercent))
            .AddDepositPercent(new Percent(depositPercent))
            .AddLimitForDubiousClient(new Limit(limitForDubiousClient))
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

    internal BaseAccount CreateAccount(TypeOfBankAccount typeOfBankAccount, IClient client, decimal amount)
    {
        return typeOfBankAccount switch
        {
            TypeOfBankAccount.Credit => AccountFactory.CreateCreditAccount(client, amount, _bankConfiguration),
            TypeOfBankAccount.Debit => AccountFactory.CreateDebitAccount(client, amount, _bankConfiguration),
            TypeOfBankAccount.Deposit => AccountFactory.CreateDepositAccount(client, amount, _bankConfiguration),
            _ => throw new ArgumentOutOfRangeException(nameof(typeOfBankAccount), typeOfBankAccount, null)
        };
    }

    internal BankTransaction Income(Guid accountId, decimal sum)
    {
        return RunningCommand(new Income(sum), accountId);
    }

    internal BankTransaction Withdraw(Guid accountId, decimal sum)
    {
        return RunningCommand(new Withdraw(sum), accountId);
    }

    private BankTransaction RunningCommand(ICommand command, Guid accountId)
    {
        BaseAccount account = _bankAccounts.FirstOrDefault(x => x.Id.Equals(accountId)) ?? throw new Exception();
        var transaction = new BankTransaction(DateTime.Now, account, command);
        transaction.Do();
        return transaction;
    }
}