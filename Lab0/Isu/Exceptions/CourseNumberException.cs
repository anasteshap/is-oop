namespace Isu.Exceptions;

public class CourseNumberException : Exception
{
    public CourseNumberException(string? message = "Course isn't exist")
        : base(message) { }
}