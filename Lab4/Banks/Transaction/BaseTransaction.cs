using Banks.Accounts;
using Banks.Accounts.Commands;

namespace Banks.Transaction;

public class BaseTransaction : BankTransaction
{
    public BaseTransaction(ICommand command)
        : base(command)
    {
    }
}