using Banks.Accounts;
using Banks.Accounts.AccountConfigurations;
using Banks.Accounts.Commands;
using Banks.DateTimeProvider;
using Banks.Exceptions;
using Banks.Interfaces;
using Banks.Transaction;

namespace Banks.Entities;

public class Bank
{
    private readonly List<BaseAccount> _bankAccounts = new ();
    private readonly List<Observer.IObserver<string>> _subscribers = new ();
    private readonly BankConfiguration _bankConfiguration;
    private readonly IClock _clock;

    public Bank(string name, IClock clock, BankConfiguration bankConfiguration)
    {
        if (string.IsNullOrEmpty(name))
            throw new Exception();
        ArgumentNullException.ThrowIfNull(nameof(clock));
        ArgumentNullException.ThrowIfNull(nameof(bankConfiguration));

        Id = Guid.NewGuid();
        Name = name;
        _clock = clock;
        _bankConfiguration = bankConfiguration;
    }

    public Guid Id { get; }
    public string Name { get; }
    public IReadOnlyCollection<BaseAccount> GetAccounts => _bankAccounts;
    public BaseAccount? FindAccount(Guid accountId) => _bankAccounts.SingleOrDefault(x => x.Id.ToString().Equals(accountId.ToString()));
    public BaseAccount GetAccount(Guid accountId) => FindAccount(accountId) ?? throw AccountException.AccountDoesNotExist(accountId);

    public void Subscribe(Observer.IObserver<string> observer)
    {
        ArgumentNullException.ThrowIfNull(nameof(observer));
        if (_subscribers.Contains(observer))
        {
            throw ObserverException.SubscribeAlreadyExists();
        }

        _subscribers.Add(observer);
    }

    public void Unsubscribe(Observer.IObserver<string> observer)
    {
        ArgumentNullException.ThrowIfNull(nameof(observer));
        if (!_subscribers.Remove(observer))
        {
            throw ObserverException.SubscribeDoesNotExist();
        }
    }

    public BaseAccount CreateAccount(TypeOfBankAccount typeOfBankAccount, IClient client, TimeSpan? endOfPeriod = null)
    {
        BaseAccount account = typeOfBankAccount switch
        {
            TypeOfBankAccount.Credit => new CreditAccount(client, _bankConfiguration),
            TypeOfBankAccount.Debit => new DebitAccount(_clock, client, _bankConfiguration),
            TypeOfBankAccount.Deposit => new DepositAccount(_clock, client, _bankConfiguration, endOfPeriod),
            _ => throw new ArgumentOutOfRangeException(nameof(typeOfBankAccount), typeOfBankAccount, null)
        };

        _bankAccounts.Add(account);
        return account;
    }

    public void ChangeDebitPercent(decimal percent)
    {
        _bankConfiguration.DebitAccountConfiguration.SetDebitPercent(percent);
        Notify(TypeOfBankAccount.Debit, $"New debit percent: {percent}%");
    }

    public void ChangeDepositPercent(List<DepositPercent> depositPercents)
    {
        _bankConfiguration.DepositAccountConfiguration.SetDepositPercents(depositPercents);
        string percents = string.Empty;
        depositPercents.ForEach(x => percents += $"{x.LeftBorder} - {x.RightBorder}: {x.Percent.Value * 100}\n");
        Notify(TypeOfBankAccount.Deposit, $"New deposit percents:\n {percents}");
    }

    public void ChangeCreditCommission(decimal commission)
    {
        _bankConfiguration.CreditAccountConfiguration.SetCreditCommission(commission);
        Notify(TypeOfBankAccount.Credit, $"New credit commission: {commission}");
    }

    public void ChangeCreditLimit(decimal creditLimit)
    {
        _bankConfiguration.CreditAccountConfiguration.SetCreditLimit(creditLimit);
        Notify(TypeOfBankAccount.Credit, $"New credit limit: {creditLimit}");
    }

    public void ChangeDepositTimeSpan(TimeSpan timeSpan) // в консоль добавить
    {
        _bankConfiguration.DepositAccountConfiguration.SetTimeSpan(timeSpan);
        Notify(TypeOfBankAccount.Credit, $"New count of days of timeSpan: {timeSpan.Days}");
    }

    public void ChangeLimitForDubiousClient(decimal limitForDubiousClient)
    {
        _bankConfiguration.SetLimitForDubiousClient(limitForDubiousClient);
        Notify(TypeOfBankAccount.Credit, $"New limit for dubious client: {limitForDubiousClient}");
        Notify(TypeOfBankAccount.Debit, $"New limit for dubious client: {limitForDubiousClient}");
        Notify(TypeOfBankAccount.Deposit, $"New limit for dubious client: {limitForDubiousClient}");
    }

    public BankTransaction Income(Guid accountId, decimal sum)
    {
        BaseAccount account = _bankAccounts.FirstOrDefault(x => x.Id.ToString().Equals(accountId.ToString())) ?? throw new Exception();
        var transaction = new BaseTransaction(new Income(account, sum));
        transaction.DoTransaction();
        account.SaveChanges(transaction);
        return transaction;
    }

    public BankTransaction Withdraw(Guid accountId, decimal sum)
    {
        BaseAccount account = _bankAccounts.FirstOrDefault(x => x.Id.Equals(accountId)) ?? throw new Exception();
        var transaction = new BaseTransaction(new Withdraw(account, sum));
        transaction.DoTransaction();
        account.SaveChanges(transaction);
        return transaction;
    }

    private void Notify(TypeOfBankAccount selectType, string data)
    {
        _bankAccounts
            .Where(acc => acc.Type.Equals(selectType))
            .Where(acc => _subscribers.Contains(acc.Client))
            .Distinct()
            .Select(x => x.Client)
            .ToList()
            .ForEach(x => x.Update(data));
    }
}