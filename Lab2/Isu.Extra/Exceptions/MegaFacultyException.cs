namespace Isu.Extra.Exceptions;

public class MegaFacultyException : Exception
{
    private MegaFacultyException(string message)
        : base(message) { }

    public static MegaFacultyException InvalidName()
    {
        return new MegaFacultyException("Invalid data");
    }
}