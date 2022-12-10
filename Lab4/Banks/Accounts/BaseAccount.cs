using Banks.Exceptions;
using Banks.Interfaces;
using Banks.Transaction;

namespace Banks.Accounts;

public abstract class BaseAccount
{
    private readonly List<BankTransaction> _transactions = new ();
    protected BaseAccount(IClient client, TypeOfBankAccount type)
    {
        ArgumentNullException.ThrowIfNull(nameof(client));
        Type = type;
        Id = Guid.NewGuid();
        Client = client;
    }

    public TypeOfBankAccount Type { get; }
    public Guid Id { get; }
    public IClient Client { get; }
    public decimal Balance { get; protected set; }
    public IReadOnlyCollection<BankTransaction> GetAllTransaction => _transactions;

    public BankTransaction GetTransaction(Guid transactionId)
        => _transactions.FirstOrDefault(x => x.Id.Equals(transactionId)) ?? throw TransactionException.TransactionDoesNotExist(transactionId);

    public void IncreaseAmount(decimal sum)
    {
        if (sum <= 0)
            throw TransactionException.NegativeAmount();

        Balance += sum;
    }

    public abstract void DecreaseAmount(decimal sum);

    public abstract void AccountDailyPayoff();

    public void SaveChanges(BankTransaction baseTransaction)
    {
        if (_transactions.Contains(baseTransaction))
            throw TransactionException.TransactionAlreadyExists(baseTransaction.Id);

        _transactions.Add(baseTransaction);
    }
}