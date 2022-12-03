using Banks.Accounts.Commands;

namespace Banks.Transaction;

public class BaseTransaction : BankTransaction
{
    public BaseTransaction(IBalanceOperationCommand command)
        : base(command)
    {
    }
}