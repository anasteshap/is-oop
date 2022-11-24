namespace Banks.Accounts.Commands;

public interface ITransactionCommand
{
    Guid Id { get; }
    void Execute();
    void Cancel();
}