using Banks.Accounts;
using Banks.Accounts.Commands;

namespace Banks.Transaction;

public class BankTransaction
{
    private readonly ICommand _command;
    private readonly BaseAccount _account;

    public BankTransaction(DateTime transactionTime, BaseAccount account, ICommand command)
    {
        TransactionInformation = new TransactionInformation(transactionTime, State.Started);
        _account = account ?? throw new ArgumentNullException();
        _command = command ?? throw new ArgumentNullException();
    }

    public Guid Id { get; } = Guid.NewGuid();
    public TransactionInformation TransactionInformation { get; }
    public void Do()
    {
        if (TransactionInformation.State is not State.Started or State.Canceled)
        {
            throw new Exception();
        }

        _command.Execute(_account);
        TransactionInformation.State = State.Ended;
        _account.SaveChanges(this);
    }

    public void Undo()
    {
        if (TransactionInformation.State is not State.Ended or State.Canceled)
        {
            throw new Exception();
        }

        _command.Cancel(_account);
        TransactionInformation.State = State.Canceled;
    }
}