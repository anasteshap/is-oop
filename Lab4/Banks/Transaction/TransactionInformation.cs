namespace Banks.Transaction;

public class TransactionInformation
{
    public TransactionInformation(DateTime transactionTime, State state)
    {
        TransactionTime = transactionTime;
        State = state;
    }

    public DateTime TransactionTime { get; }
    public State State { get; set; }
}