using Banks.Exceptions;

namespace Banks.Accounts.Commands;

public class Income : IBalanceOperationCommand
{
    private readonly BaseAccount _account;
    private readonly decimal _sum;

    public Income(BaseAccount account, decimal sum)
    {
        ArgumentNullException.ThrowIfNull(nameof(account));
        _account = account;
        _sum = sum < 0 ? throw TransactionException.NegativeAmount() : sum;
    }

    public void Execute()
    {
        _account.IncreaseAmount(_sum);
    }

    public void Cancel()
    {
        _account.DecreaseAmount(_sum);
    }
}