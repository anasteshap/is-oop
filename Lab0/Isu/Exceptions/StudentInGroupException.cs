using Isu.Entities;
namespace Isu.Exceptions;

public class StudentInGroupException : Exception
{
    private StudentInGroupException(string message)
        : base(message) { }

    public static StudentInGroupException StudentAlreadyInGroup(Student student)
    {
        return new StudentInGroupException($"Student {student.Name} {student.Id} has already been in the group");
    }
}