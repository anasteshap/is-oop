namespace Isu.Extra.Entities;

public class Teacher
{
    public Teacher(string name)
    {
        if (name is null)
        {
            throw new Exception();
        }

        Name = name;
    }

    public string Name { get; }
}