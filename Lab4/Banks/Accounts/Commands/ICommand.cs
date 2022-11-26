namespace Banks.Accounts.Commands;

public interface ICommand
{
    void Execute();
    void Cancel();
}