namespace Isu.Extra.Models;

public class MegaFaculty
{
    private readonly List<char> _faculties;

    public MegaFaculty(string name, List<char> faculties)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new Exception();
        }

        if (faculties.Count < 1)
        {
            throw new Exception();
        }

        Name = name;
        _faculties = faculties;
    }

    public string Name { get; }
    public IReadOnlyList<char> Faculties() => _faculties.AsReadOnly();
}