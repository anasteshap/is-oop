namespace Isu.Extra.Exceptions;

public class ExtraStudentException : Exception
{
    private ExtraStudentException(string message)
        : base(message) { }

    public static ExtraStudentException InvalidSchedule()
    {
        return new ExtraStudentException("Invalid data");
    }

    public static ExtraStudentException InvalidStudent()
    {
        return new ExtraStudentException("Invalid data");
    }

    public static ExtraStudentException StudentAlreadyExist()
    {
        return new ExtraStudentException("StudentAlreadyExist");
    }
}