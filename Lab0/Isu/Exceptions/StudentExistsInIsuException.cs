namespace Isu.Exceptions;

public class StudentExistsInIsuException : Exception
{
    public StudentExistsInIsuException(string? message = "Student is already exist in Isu")
        : base(message)
    {
    }
}