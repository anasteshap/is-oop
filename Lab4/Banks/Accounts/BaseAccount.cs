using Banks.Accounts.Commands;
using Banks.Interfaces;
using Banks.Transaction;

namespace Banks.Accounts;

public abstract class BaseAccount
{
    private readonly List<BankTransaction> _transactions = new ();
    protected BaseAccount(IClient client, TypeOfBankAccount type)
    {
        Type = type;
        Id = Guid.NewGuid();
        Client = client ?? throw new ArgumentNullException();
    }

    public TypeOfBankAccount Type { get; }
    public Guid Id { get; }
    public IClient Client { get; }
    public decimal Balance { get; protected set; }
    public IReadOnlyCollection<BankTransaction> GetAllTransaction => _transactions;

    public BankTransaction GetTransaction(Guid transactionId)
        => _transactions.FirstOrDefault(x => x.Id.Equals(transactionId)) ?? throw new Exception();

    public void IncreaseAmount(decimal sum)
    {
        if (sum <= 0)
            throw new Exception("Sum can't be <= 0");

        Balance += sum;
    }

    public abstract void DecreaseAmount(decimal sum);

    public virtual void AccountDailyPayoff()
        => throw new NotImplementedException();

    public void SaveChanges(BankTransaction baseTransaction)
    {
        if (_transactions.Contains(baseTransaction))
        {
            throw new Exception();
        }

        _transactions.Add(baseTransaction);
    }
}