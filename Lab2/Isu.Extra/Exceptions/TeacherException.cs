using Isu.Extra.Entities;

namespace Isu.Extra.Exceptions;

public class TeacherException : Exception
{
    private TeacherException(string message)
        : base(message) { }

    public static TeacherException InvalidName()
    {
        return new TeacherException("Invalid data");
    }
}