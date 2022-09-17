using Isu.Models;
namespace Isu.Exceptions;

public class GroupNameException : Exception
{
    private GroupNameException(string message)
        : base(message) { }

    public static GroupNameException InvalidGroupName(string name)
    {
        return new GroupNameException($"{name} - invalid name of group");
    }
}