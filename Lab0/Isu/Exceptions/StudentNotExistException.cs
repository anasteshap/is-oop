namespace Isu.Exceptions;

public class StudentNotExistException : Exception
{
    public StudentNotExistException(string? message = "Student isn't exists")
        : base(message)
    {
    }
}