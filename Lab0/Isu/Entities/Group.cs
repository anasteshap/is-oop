using Isu.Exceptions;
using Isu.Models;

namespace Isu.Entities;

public class Group
{
    private const int MaxCountOfPeople = 23;

    public Group(GroupName groupName)
    {
        GroupName = groupName;
        Students = new List<Student>();
        CourseNumber = groupName.CourseNumber;
    }

    public GroupName GroupName { get; }
    public List<Student> Students { get; }
    public CourseNumber CourseNumber { get; }

    public void AddStudent(Student student)
    {
        if (Students.Count == MaxCountOfPeople)
        {
            throw new FullGroupException($"Group {GroupName} is full");
        }

        if (StudentInGroup(student))
        {
            throw new StudentInGroupException($"Student {student.Name} ({student.Id}) is already in group");
        }

        Students.Add(student);
    }

    public void DeleteStudent(Student student)
    {
        if (!StudentInGroup(student))
        {
            throw new StudentInGroupException($"Student {student.Name} ({student.Id}) isn't in the group");
        }

        Students.Remove(student);
    }

    public bool StudentInGroup(Student student)
    {
        return Students.Contains(student);
    }
}