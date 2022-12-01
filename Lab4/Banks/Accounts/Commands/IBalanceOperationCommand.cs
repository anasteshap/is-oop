namespace Banks.Accounts.Commands;

public interface IBalanceOperationCommand : ICommand
{
    void Cancel();
}