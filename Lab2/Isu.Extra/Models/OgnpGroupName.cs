namespace Isu.Extra.Models;

public class OgnpGroupName
{
    public OgnpGroupName(string name)
    {
        if (name is null)
        {
            throw new Exception();
        }

        Name = name;
    }

    public string Name { get; }
}