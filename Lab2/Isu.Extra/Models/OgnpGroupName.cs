using Isu.Extra.Exceptions;

namespace Isu.Extra.Models;

public class OgnpGroupName
{
    public OgnpGroupName(string name)
    {
        if (name is null)
        {
            throw OgnpException.InvalidGroupName();
        }

        Name = name;
    }

    public string Name { get; }
}