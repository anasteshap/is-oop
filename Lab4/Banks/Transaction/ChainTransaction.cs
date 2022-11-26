using Banks.Accounts.Commands;

namespace Banks.Transaction;

public class ChainTransaction : BankTransaction
{
    public ChainTransaction(ICommand command)
        : base(command)
    {
    }

    public BankTransaction? Previous { get; internal set; } = null;
    public BankTransaction? Next { get; private set; } = null;
    public BankTransaction SetNext(ChainTransaction nextTransaction)
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