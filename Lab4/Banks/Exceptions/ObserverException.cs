namespace Banks.Exceptions;

public class ObserverException : Exception
{
    private ObserverException(string message)
            : base(message) { }

    public static ObserverException SubscribeAlreadyExists()
    {
        return new ObserverException($"Subscribe already exists");
    }

    public static ObserverException SubscribeDoesNotExist()
    {
        return new ObserverException($"Subscribe doesn't exist");
    }
}