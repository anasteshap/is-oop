using Isu.Exceptions;
using Isu.Models;

namespace Isu.Entities;

public class Group
{
    private const int MaxCountOfPeople = 23;
    private readonly List<Student> _students;

    public Group(GroupName groupName)
    {
        GroupName = groupName;
        _students = new List<Student>();
    }

    public GroupName GroupName { get; }
    public IReadOnlyList<Student> Students { get => _students.AsReadOnly(); }

    public void AddStudent(Student student)
    {
        if (Students.Count == MaxCountOfPeople)
        {
            throw FullGroupException.GroupIsFull(GroupName);
        }

        if (Students.Contains(student))
        {
            throw StudentInGroupException.StudentAlreadyInGroup(student);
        }

        _students.Add(student);
    }

    public void DeleteStudent(Student student)
    {
        if (!_students.Remove(student))
        {
            throw StudentNotInGroupException.StudentNotInGroup(student);
        }
    }

    public bool StudentInGroup(Student student)
    {
        return Students.Contains(student);
    }
}