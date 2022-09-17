using Isu.Models;
namespace Isu.Exceptions;

public class CourseNumberException : Exception
{
    private CourseNumberException(string message)
        : base(message) { }

    public static CourseNumberException InvalidCourseNumber(int courseNumber)
    {
        return new CourseNumberException($"Course {courseNumber} isn't exist");
    }
}