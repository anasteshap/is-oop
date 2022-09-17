using Isu.Exceptions;

namespace Isu.Models;

public class GroupName
{
    public GroupName(string name, CourseNumber courseNumber)
    {
        CheckGroupName.CheckLengthName(name);
        Name = name;
        Faculty = CheckGroupName.CheckFaculty(name);
        Grade = CheckGroupName.CheckGrade(name);
        CourseNumber = courseNumber;
        GroupNumber = int.Parse(name.Substring(3, 2));
        Specialisation = CheckGroupName.CheckSpecialisation(name);
    }

    public string Name { get; }
    public char Faculty { get; }
    public int Grade { get; }
    public CourseNumber CourseNumber { get; }
    public int GroupNumber { get; }
    public char Specialisation { get; }
}