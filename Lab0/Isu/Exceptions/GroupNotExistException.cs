using Isu.Models;
namespace Isu.Exceptions;

public class GroupNotExistException : Exception
{
    private GroupNotExistException(string message)
        : base(message) { }

    public static GroupNotExistException GroupNotExist(GroupName groupName)
    {
        return new GroupNotExistException($"Group {groupName.Name} isn't exist");
    }
}