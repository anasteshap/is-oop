namespace Banks.Exceptions;

public class BankException : Exception
{
    private BankException(string message)
        : base(message) { }

    public static BankException BankAlreadyExists(string name)
    {
        return new BankException($"Bank with name: {name} already exists");
    }

    public static BankException BankDoesNotExist(string name)
    {
        return new BankException($"Bank with name: {name} doesn't exist");
    }
}