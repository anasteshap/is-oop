using Banks.Transaction;

namespace Banks.Accounts.Commands;

public class TransferMoney : ITransactionCommand
{
    private readonly ITransactionCommand _command1;
    private readonly ITransactionCommand _command2;

    public TransferMoney(ITransactionCommand withdraw, ITransactionCommand income)
    {
        _command1 = withdraw;
        _command2 = income;
    }

    public Guid Id { get; } = Guid.NewGuid();

    public void Execute()
    {
        _command1.Execute();
        _command2.Execute();
    }

    public void Cancel()
    {
        throw new NotImplementedException();
    }
}