using Isu.Entities;
namespace Isu.Exceptions;

public class StudentNotInGroupException : Exception
{
    private StudentNotInGroupException(string message)
        : base(message) { }

    public static StudentNotInGroupException StudentNotInGroup(Student student)
    {
        return new StudentNotInGroupException($"Student {student.Name} {student.Id} isn't in the group");
    }
}