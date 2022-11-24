using Banks.Transaction;

namespace Banks.Accounts.Commands;

public class Income : ICommand
{
    private readonly decimal _sum;
    public Income(decimal sum)
    {
        _sum = sum < 0 ? throw new Exception() : sum;
    }

    public void Execute(BaseAccount account)
    {
        account.IncreaseAmount(_sum);
    }

    public void Cancel(BaseAccount account)
    {
        account.IncreaseAmount(_sum);
    }
}