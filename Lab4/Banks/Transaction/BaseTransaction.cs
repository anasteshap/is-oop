using Banks.Accounts;
using Banks.Accounts.Commands;

namespace Banks.Transaction;

public abstract class BaseTransaction
{
    private readonly ITransactionCommand _transactionCommand;

    protected BaseTransaction(DateTime transactionTime, ITransactionCommand transactionCommand)
    {
        TransactionInformation = new TransactionInformation(transactionTime, State.Started);
        _transactionCommand = transactionCommand ?? throw new ArgumentNullException();
    }

    public Guid Id { get; } = Guid.NewGuid();
    public TransactionInformation TransactionInformation { get; }
    public virtual void DoTransaction()
    {
        if (TransactionInformation.State is not State.Started or State.Canceled)
        {
            throw new Exception();
        }

        _transactionCommand.Execute();
        TransactionInformation.State = State.Ended;
    }

    public virtual void Undo()
    {
        if (TransactionInformation.State is not State.Ended or State.Canceled)
        {
            throw new Exception();
        }

        _transactionCommand.Cancel();
        TransactionInformation.State = State.Canceled;
    }
}