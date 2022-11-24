using Banks.Accounts;
using Banks.Accounts.Commands;

namespace Banks.Transaction;

public class BankTransaction : BaseTransaction
{
    public BankTransaction(DateTime transactionTime, ITransactionCommand transactionCommand)
        : base(transactionTime, transactionCommand)
    {
    }
}