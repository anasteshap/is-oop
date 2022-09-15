namespace Isu.Exceptions;

public class StudentInGroupException : Exception
{
    public StudentInGroupException(string? message = "Student has already been in the group")
        : base(message) { }
}