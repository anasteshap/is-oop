using Isu.Extra.Exceptions;

namespace Isu.Extra.Models;

public class OgnpName
{
    public OgnpName(string name, string megaFaculty)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(megaFaculty))
        {
            throw OgnpException.InvalidName();
        }

        Name = name;
        MegaFaculty = megaFaculty;
    }

    public string Name { get; }
    public string MegaFaculty { get; }
}