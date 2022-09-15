namespace Isu.Exceptions;

public class StudentNotInGroupException : Exception
{
    public StudentNotInGroupException(string? message = "Student with this id isn't in the group")
        : base(message)
    {
    }
}