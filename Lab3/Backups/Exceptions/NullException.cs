namespace Backups.Exceptions;

public class NullException : Exception
{
    private NullException(string message)
        : base(message) { }

    public static NullException InvalidName() => new NullException("Name is null");
}