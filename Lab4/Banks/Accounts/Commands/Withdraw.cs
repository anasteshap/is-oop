using Banks.Interfaces;

namespace Banks.Accounts.Commands;

public class Withdraw : ICommand
{
    private readonly decimal _sum;
    public Withdraw(decimal sum)
    {
        _sum = sum < 0 ? throw new Exception() : sum;
    }

    public void Execute(BaseAccount account)
    {
        account.DecreaseAmount(_sum);
    }

    public void Cancel(BaseAccount account)
    {
        account.IncreaseAmount(_sum);
    }
}