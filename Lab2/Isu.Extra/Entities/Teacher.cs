using Isu.Extra.Exceptions;

namespace Isu.Extra.Entities;

public class Teacher
{
    public Teacher(string name)
    {
        if (name is null)
        {
            throw TeacherException.InvalidName();
        }

        Name = name;
    }

    public string Name { get; }
}