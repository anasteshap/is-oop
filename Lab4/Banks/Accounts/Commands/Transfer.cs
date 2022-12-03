namespace Banks.Accounts.Commands;

public class Transfer : IBalanceOperationCommand
{
    private readonly BaseAccount _toAccount;
    private readonly BaseAccount _fromAccount;
    private readonly decimal _sum;

    public Transfer(BaseAccount toAccount, BaseAccount fromAccount, decimal sum)
    {
        _toAccount = toAccount;
        _fromAccount = fromAccount;
        _sum = sum < 0 ? throw new Exception() : sum;
    }

    public void Execute()
    {
        try
        {
            _fromAccount.DecreaseAmount(_sum);
        }
        catch (Exception)
        {
            _fromAccount.IncreaseAmount(_sum);
            throw new Exception("не удалось снять деньги с 1 счёта");
        }

        _toAccount.IncreaseAmount(_sum);
    }

    public void Cancel()
    {
        try
        {
            _toAccount.DecreaseAmount(_sum);
        }
        catch (Exception)
        {
            _toAccount.IncreaseAmount(_sum);
            throw new Exception("не удалось снять деньги с 1 счёта");
        }

        _fromAccount.IncreaseAmount(_sum);
    }
}