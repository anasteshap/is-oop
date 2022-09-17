using Isu.Models;
namespace Isu.Exceptions;

public class FullGroupException : Exception
{
    private FullGroupException(string message)
        : base(message) { }

    public static FullGroupException GroupIsFull(GroupName groupName)
    {
        return new FullGroupException($"Group {groupName.Name} is full");
    }
}