namespace Backups.Exceptions;

public class VisitorException : Exception
{
    private VisitorException(string message)
        : base(message) { }

    public static VisitorException InvalidZipObjects()
    {
        return new VisitorException($"Structure of ZipObjects is invalid");
    }
}