using Banks.Accounts.Commands;
using Banks.Interfaces;
using Banks.Transaction;

namespace Banks.Accounts;

public abstract class BaseAccount
{
    private readonly List<BankTransaction> _transactions = new ();
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

    public BankTransaction GetTransaction(Guid transactionId)
        => _transactions.FirstOrDefault(x => x.Id.Equals(transactionId)) ?? throw new Exception();

    public abstract void IncreaseAmount(decimal sum);
    public abstract void DecreaseAmount(decimal sum);

    public void SaveChanges(BankTransaction bankTransaction)
    {
        if (_transactions.Contains(bankTransaction))
        {
            throw new Exception();
        }

        _transactions.Add(bankTransaction);
    }
}