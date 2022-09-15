using Isu.Exceptions;

namespace Isu.Models;

public class GroupName
{
    public GroupName(string name, CourseNumber courseNumber)
    {
        if (name.Length != 5 && name.Length != 6)
        {
            throw new GroupNameException("Length of nameGroup");
        }

        if (!IsCorrectFaculty(name[0]))
        {
            throw new GroupNameException("Faculty of nameGroup");
        }

        if (!IsCorrectGrade(int.Parse(name[1].ToString())))
        {
            throw new GroupNameException("Grade of nameGroup");
        }

        Name = name;
        Faculty = name[0];
        Grade = int.Parse(name[1].ToString());
        CourseNumber = courseNumber;
        GroupNumber = int.Parse(name.Substring(3, 2));
        Specialisation = ' ';

        if (name.Length == 6)
        {
            Specialisation = name[5];
        }
    }

    public string Name { get; }
    public char Faculty { get; }
    public int Grade { get; }
    public CourseNumber CourseNumber { get; }
    public int GroupNumber { get; }
    public char Specialisation { get; }

    private static bool IsCorrectFaculty(char name)
    {
        if (name - 'A' > 0 || name - 'Z' <= 0)
            return true;
        return false;
    }

    private static bool IsCorrectGrade(int number)
    {
        return number == 3;
    }
}