using Banks.Accounts.Commands;
using Banks.Interfaces;
using Banks.Transaction;

namespace Banks.Accounts;

public abstract class BaseAccount
{
    private readonly List<BaseTransaction> _transactions = new ();
    protected BaseAccount(IClient client, decimal amount)
    {
        if (amount < 0)
        {
            throw new Exception();
        }

        Id = Guid.NewGuid();
        Client = client ?? throw new ArgumentNullException();
        Amount = amount;
    }

    public Guid Id { get; }
    public IClient Client { get; }
    public decimal Amount { get; protected set; }

    public BaseTransaction GetTransaction(Guid transactionId)
        => _transactions.FirstOrDefault(x => x.Id.Equals(transactionId)) ?? throw new Exception();

    public void IncreaseAmount(decimal sum)
    {
        if (sum <= 0)
        {
            throw new Exception();
        }

        Amount += sum;
    }

    public abstract void DecreaseAmount(decimal sum);

    /*public void SaveChanges(ITransactionCommand transactionCommand)
    {
        if (_transactions.Contains(transactionCommand))
        {
            throw new Exception();
        }

        _transactions.Add(transactionCommand);
    }*/
}