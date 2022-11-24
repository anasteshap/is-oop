namespace Banks.Accounts.Commands;

public interface ICommand
{
    void Execute(BaseAccount account);
    void Cancel(BaseAccount account);
}