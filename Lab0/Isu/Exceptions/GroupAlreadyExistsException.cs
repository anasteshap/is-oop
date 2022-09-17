using Isu.Models;
namespace Isu.Exceptions;

public class GroupAlreadyExistsException : Exception
{
    private GroupAlreadyExistsException(string message)
        : base(message) { }

    public static GroupAlreadyExistsException GroupAlreadyExists(GroupName groupName)
    {
        return new GroupAlreadyExistsException($"Group {groupName.Name} is already exist");
    }
}