namespace Isu.Exceptions;

public class GroupAlreadyExistsException : Exception
{
    public GroupAlreadyExistsException(string? message = "Group is already exist")
        : base(message)
    {
    }
}