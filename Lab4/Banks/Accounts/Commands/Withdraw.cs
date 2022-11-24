using Banks.Interfaces;

namespace Banks.Accounts.Commands;

public class Withdraw : ITransactionCommand
{
    private readonly BaseAccount _account;
    private readonly decimal _sum;
    public Withdraw(BaseAccount account, decimal sum)
    {
        _account = account;
        _sum = sum < 0 ? throw new Exception() : sum;
    }

    public Guid Id { get; } = Guid.NewGuid();

    public void Execute()
    {
        _account.DecreaseAmount(_sum);
    }

    public void Cancel()
    {
        _account.IncreaseAmount(_sum);
    }
}