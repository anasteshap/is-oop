using Banks.Accounts.Commands;

namespace Banks.Transaction;

public class ChainTransaction : BaseTransaction
{
    public ChainTransaction(DateTime transactionTime, ITransactionCommand transactionCommand)
        : base(transactionTime, transactionCommand)
    {
    }

    public BaseTransaction? Previous { get; internal set; } = null;
    public BaseTransaction? Next { get; private set; } = null;
    public BaseTransaction SetNext(ChainTransaction nextTransaction)
    {
        nextTransaction.Previous = this;
        Next = nextTransaction;
        return nextTransaction;
    }

    public override void DoTransaction()
    {
        base.DoTransaction();
        Next?.DoTransaction();
    }

    public override void Undo()
    {
        base.Undo();
        Previous?.Undo();
    }
}