namespace Isu.Extra.Exceptions;

public class LessonException : Exception
{
    private LessonException(string message)
        : base(message)
    {
    }

    public static LessonException InvalidName()
    {
        return new LessonException("Invalid data");
    }

    public static LessonException InvalidAudience()
    {
        return new LessonException("Invalid data");
    }

    public static LessonException InvalidLessonTime()
    {
        return new LessonException("Invalid data");
    }
}