using Banks.Transaction;

namespace Banks.Accounts.Commands;

public class TransferMoney : ICommand
{
    private readonly ICommand _command1;
    private readonly ICommand _command2;

    public TransferMoney(ICommand withdraw, ICommand income)
    {
        _command1 = withdraw;
        _command2 = income;
    }

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