namespace Isu.Exceptions;

public class GroupNotExistException : Exception
{
    public GroupNotExistException(string? message = "Group isn't exist")
        : base(message)
    {
    }
}