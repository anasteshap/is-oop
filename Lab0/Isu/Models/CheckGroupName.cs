using Isu.Exceptions;
namespace Isu.Models;

public class CheckGroupName
{
    public static void CheckLengthName(string groupName)
    {
        if (groupName.Length != 5 && groupName.Length != 6)
        {
            throw GroupNameException.InvalidGroupName(groupName);
        }
    }

    public static char CheckFaculty(string groupName)
    {
        var faculty = groupName[0];
        if (faculty - 'A' < 0 || faculty - 'Z' > 0)
        {
            throw GroupNameException.InvalidGroupName(groupName);
        }

        return faculty;
    }

    public static int CheckGrade(string groupName)
    {
        var grade = int.Parse(groupName[1].ToString());
        if (grade != 3)
        {
            throw GroupNameException.InvalidGroupName(groupName);
        }

        return grade;
    }

    public static char CheckSpecialisation(string groupName)
    {
        if (groupName.Length == 6)
        {
            return groupName[5];
        }

        return ' ';
    }
}