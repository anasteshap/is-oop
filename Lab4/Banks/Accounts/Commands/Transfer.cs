using Banks.Exceptions;

namespace Banks.Accounts.Commands;

public class Transfer : IBalanceOperationCommand
{
    private readonly BaseAccount _toAccount;
    private readonly BaseAccount _fromAccount;
    private readonly decimal _sum;

    public Transfer(BaseAccount toAccount, BaseAccount fromAccount, decimal sum)
    {
        ArgumentNullException.ThrowIfNull(nameof(toAccount));
        ArgumentNullException.ThrowIfNull(nameof(fromAccount));
        _toAccount = toAccount;
        _fromAccount = fromAccount;
        _sum = sum < 0 ? throw TransactionException.NegativeAmount() : sum;
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
            throw TransactionException.FailedTransaction("Couldn't withdraw money from 1 account");
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
            throw TransactionException.FailedTransaction("Couldn't withdraw money from 2 account");
        }

        _fromAccount.IncreaseAmount(_sum);
    }
}