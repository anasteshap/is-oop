using Isu.Entities;
using Isu.Exceptions;
using Isu.Models;
using Isu.Services;

namespace Isu.Test;

public class IsuService : IIsuService
{
    private readonly IdGenerator _idGenerator = new IdGenerator();
    private readonly HashSet<Student> _students = new HashSet<Student>();
    private readonly HashSet<Group> _groups = new HashSet<Group>();

    public Group AddGroup(GroupName name)
    {
        var group = new Group(name);

        if (_groups.Contains(group))
        {
            throw new GroupAlreadyExistsException($"Group {name.Name} is already exist");
        }

        _groups.Add(group);
        return group;
    }

    public Student AddStudent(Group group, string name)
    {
        var student = new Student(name, _idGenerator.Id, group.GroupName);
        if (_students.Contains(student))
        {
            throw new StudentExistsInIsuException($"Student {student.Name} is already exist in Isu");
        }

        if (group.Students.Contains(student))
        {
            throw new StudentInGroupException($"Student {student.Name} is already in group");
        }

        group.AddStudent(student);
        _students.Add(student);
        return student;
    }

    public Student GetStudent(int id)
    {
        return FindStudent(id) ??
               throw new StudentNotExistException($"Student doesn't exist, id {id} is invalid");
    }

    public Student? FindStudent(int id)
    {
        return _students.FirstOrDefault(x => x.Id == id);
    }

    public List<Student> FindStudents(GroupName groupName)
    {
        return _groups.FirstOrDefault(x => x.GroupName == groupName)?.Students ?? new List<Student>();
    }

    public List<Student> FindStudents(CourseNumber courseNumber)
    {
        return _students.Where(x => x.GroupName.CourseNumber == courseNumber).ToList();
    }

    public Group? FindGroup(GroupName groupName)
    {
        return _groups.FirstOrDefault(x => x.GroupName == groupName);
    }

    public List<Group> FindGroups(CourseNumber courseNumber)
    {
        return _groups.Where(x => x.CourseNumber == courseNumber).ToList();
    }

    public void ChangeStudentGroup(Student student, Group newGroup)
    {
        if (FindStudent(student.Id) == null)
        {
            throw new StudentNotExistException($"Student {student.Name} doesn't exist");
        }

        var oldGroup = _groups.FirstOrDefault(x => x.GroupName == student.GroupName);
        if (oldGroup == null)
        {
            throw new GroupNotExistException($"Group {student.GroupName} doesn't exist");
        }

        if (newGroup.Students.Contains(student))
        {
            throw new StudentInGroupException($"Student {student.Name} is already in group.");
        }

        oldGroup.DeleteStudent(student);
        student.GroupName = newGroup.GroupName;
        newGroup.AddStudent(student);
    }
}