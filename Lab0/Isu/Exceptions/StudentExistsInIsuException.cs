using Isu.Models;
namespace Isu.Exceptions;

public class StudentExistsInIsuException : Exception
{
    private StudentExistsInIsuException(string message)
        : base(message) { }

    public static StudentExistsInIsuException StudentAlreadyExistsInIsu(string studentName)
    {
        return new StudentExistsInIsuException($"Student {studentName} is already exist in Isu");
    }
}