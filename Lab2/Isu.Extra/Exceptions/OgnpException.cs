using Isu.Extra.Entities;

namespace Isu.Extra.Exceptions;

public class OgnpException : Exception
{
    private OgnpException(string message)
        : base(message) { }

    public static OgnpException InvalidGroupName()
    {
        return new OgnpException("Invalid data");
    }

    public static OgnpException InvalidName()
    {
        return new OgnpException("Invalid data");
    }

    public static OgnpException LessonAlreadyExist(OgnpStream ognpStream, Lesson lesson)
    {
        return new OgnpException($"Lesson {ognpStream.Number} {lesson.Name} has already existed");
    }

    public static OgnpException StreamAlreadyExist(OgnpStream ognpStream)
    {
        return new OgnpException($"Lesson {ognpStream.Number} has already existed");
    }

    public static OgnpException OgnpAlreadyExist()
    {
        return new OgnpException("OgnpAlreadyExist");
    }
}