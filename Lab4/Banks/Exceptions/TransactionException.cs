namespace Banks.Exceptions;

public class TransactionException : Exception
{
    private TransactionException(string message)
        : base(message)
    {
    }

    public static TransactionException NegativeAmount()
    {
        return new TransactionException("Amount is < 0 or <= 0");
    }

    public static TransactionException FailedTransaction(string? message = null)
    {
        return new TransactionException($"Transaction failed\n{message}");
    }

    public static TransactionException TransactionAlreadyExists(Guid id)
    {
        return new TransactionException($"Transaction with id: {id.ToString()} already exists");
    }

    public static TransactionException TransactionDoesNotExist(Guid id)
    {
        return new TransactionException($"Transaction with id: {id.ToString()} doesn't exist");
    }

    public static TransactionException SumExceedingLimit(decimal sum, decimal limit)
    {
        return new TransactionException($"Sum {sum} exceeding the limit {limit}");
    }
}