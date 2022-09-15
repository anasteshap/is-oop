namespace Isu.Exceptions;

public class GroupNameException : Exception
{
    public GroupNameException(string? message = "This group isn't exist")
        : base(message) { }
}