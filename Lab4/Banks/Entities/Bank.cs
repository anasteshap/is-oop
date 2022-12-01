using Banks.Accounts;
using Banks.Accounts.AccountConfigurations;
using Banks.Accounts.Commands;
using Banks.Accounts.Factory;
using Banks.Interfaces;
using Banks.Observer;
using Banks.Service;
using Banks.Transaction;

namespace Banks.Entities;

public class Bank : IObservable
{
    private readonly ICentralBank _centralBank = new CentralBank();
    private readonly List<BaseAccount> _bankAccounts = new ();
    private readonly List<IObserver> _subscribers = new ();
    private readonly BankConfiguration _bankConfiguration;

    public Bank(string name, BankConfiguration bankConfiguration)
    {
        ArgumentNullException.ThrowIfNull(nameof(name));
        ArgumentNullException.ThrowIfNull(nameof(bankConfiguration));

        Name = name;
        _bankConfiguration = bankConfiguration;
        Id = Guid.NewGuid();
    }

    public Guid Id { get; }
    public string Name { get; }
    public IReadOnlyCollection<BaseAccount> GetAccounts => _bankAccounts;
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

    internal BaseAccount CreateAccount(TypeOfBankAccount typeOfBankAccount, IClient client, uint? depositPeriodInDays = null)
    {
        BaseAccount account;
        switch (typeOfBankAccount)
        {
            case TypeOfBankAccount.Credit:
                account = AccountFactory.CreateCreditAccount(client, _bankConfiguration);
                _bankAccounts.Add(account);
                break;
            case TypeOfBankAccount.Debit:
                account = AccountFactory.CreateDebitAccount(client, _bankConfiguration);
                _bankAccounts.Add(account);
                break;
            case TypeOfBankAccount.Deposit:
                account = AccountFactory.CreateDepositAccount(client, _bankConfiguration, depositPeriodInDays);
                _bankAccounts.Add(account);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(typeOfBankAccount), typeOfBankAccount, null);
        }

        return account;
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