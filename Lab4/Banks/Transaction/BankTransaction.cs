using Banks.Accounts.Commands;
using Banks.Exceptions;

namespace Banks.Transaction;

public abstract class BankTransaction
{
    private readonly IBalanceOperationCommand _command;

    protected BankTransaction(IBalanceOperationCommand command)
    {
        ArgumentNullException.ThrowIfNull(nameof(command));
        _command = command;
        TransactionState = State.Started;
        StatusMessage = $"BankTransaction {TransactionState.ToString()}";
    }

    public Guid Id { get; } = Guid.NewGuid();
    public State TransactionState { get; private set; }
    public string StatusMessage { get; private set; }

    public virtual void DoTransaction()
    {
        if (TransactionState is not State.Started or State.Canceled)
            throw TransactionException.FailedTransaction();

        try
        {
            _command.Execute();
            TransactionState = State.Ended;
            StatusMessage = $"BankTransaction {TransactionState.ToString()}";
        }
        catch (Exception e)
        {
            TransactionState = State.Failed;
            StatusMessage = $"BankTransaction {TransactionState.ToString()}: {e.Message}";
        }
    }

    public virtual void Undo()
    {
        if (TransactionState is not State.Ended or State.Canceled)
            throw TransactionException.FailedTransaction();

        try
        {
            _command.Cancel();
            TransactionState = State.Canceled;
            StatusMessage = $"BankTransaction {TransactionState.ToString()}";
        }
        catch (Exception e)
        {
            TransactionState = State.Failed;
            StatusMessage = $"BankTransaction {TransactionState.ToString()}: {e.Message}";
        }
    }
}