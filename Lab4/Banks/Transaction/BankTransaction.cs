using Banks.Accounts.Commands;

namespace Banks.Transaction;

public abstract class BankTransaction
{
    private readonly IBalanceOperationCommand _command;

    protected BankTransaction(IBalanceOperationCommand command)
    {
        _command = command ?? throw new ArgumentNullException();
        TransactionState = State.Started;
        StatusMessage = $"BankTransaction {TransactionState.ToString()}";
    }

    public Guid Id { get; } = Guid.NewGuid();
    public State TransactionState { get; private set; }
    public string StatusMessage { get; protected set; }

    public virtual void DoTransaction()
    {
        if (TransactionState is not State.Started or State.Canceled)
        {
            throw new Exception();
        }

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
        {
            throw new Exception();
        }

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