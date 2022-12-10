using Banks.Exceptions;
using Banks.Interfaces;

namespace Banks.Accounts.Commands;

public class Withdraw : IBalanceOperationCommand
{
    private readonly BaseAccount _account;
    private readonly decimal _sum;

    public Withdraw(BaseAccount account, decimal sum)
    {
        ArgumentNullException.ThrowIfNull(nameof(account));
        _account = account;
        _sum = sum < 0 ? throw TransactionException.NegativeAmount() : sum;
    }

    public void Execute()
    {
        _account.DecreaseAmount(_sum);
    }

    public void Cancel()
    {
        _account.IncreaseAmount(_sum);
    }
}