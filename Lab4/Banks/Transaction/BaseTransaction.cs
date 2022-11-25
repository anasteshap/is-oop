using Banks.Accounts;
using Banks.Accounts.Commands;

namespace Banks.Transaction;

public abstract class BaseTransaction
{
    private readonly ITransactionCommand _transactionCommand;

    protected BaseTransaction(ITransactionCommand transactionCommand)
    {
        _transactionCommand = transactionCommand ?? throw new ArgumentNullException();
        TransactionState = State.Started;
        StatusMessage = $"Transaction {TransactionState.ToString()}";
    }

    public Guid Id { get; } = Guid.NewGuid();
    public State TransactionState { get; private set; }
    public string StatusMessage { get; private set; }

    public virtual void DoTransaction()
    {
        if (TransactionState is not State.Started or State.Canceled)
        {
            throw new Exception();
        }

        try
        {
            _transactionCommand.Execute();
        }
        catch (Exception e)
        {
            TransactionState = State.Failed;
            StatusMessage = $"Transaction {TransactionState.ToString()}: {e.Message}";
        }
        finally
        {
            TransactionState = State.Ended;
            StatusMessage = $"Transaction {TransactionState.ToString()}";
        }
    }

    public virtual void Undo()
    {
        if (TransactionState is not State.Ended or State.Canceled)
        {
            throw new Exception();
        }

        try
        {
            _transactionCommand.Cancel();
        }
        catch (Exception e)
        {
            TransactionState = State.Failed;
            StatusMessage = $"Transaction {TransactionState.ToString()}: {e.Message}";
        }
        finally
        {
            TransactionState = State.Canceled;
            StatusMessage = $"Transaction {TransactionState.ToString()}";
        }
    }
}