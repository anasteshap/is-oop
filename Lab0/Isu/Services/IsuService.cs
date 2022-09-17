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
            throw GroupAlreadyExistsException.GroupAlreadyExists(name);
        }

        _groups.Add(group);
        return group;
    }

    public Student AddStudent(Group group, string name)
    {
        var student = new Student(name, _idGenerator.NewId(), group.GroupName);
        if (_students.Contains(student))
        {
            throw StudentExistsInIsuException.StudentAlreadyExistsInIsu(student.Name);
        }

        group.AddStudent(student);
        _students.Add(student);
        return student;
    }

    public Student GetStudent(int id)
    {
        Student student = FindStudent(id) ??
                throw StudentNotExistException.StudentNotExist(id);
        return student;
    }

    public Student? FindStudent(int id)
    {
        return _students.FirstOrDefault(x => x.Id == id);
    }

    public List<Student> FindStudents(GroupName groupName)
    {
        return _groups.FirstOrDefault(x => x.GroupName == groupName)?.Students.ToList() ?? new List<Student>();
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
        return _groups.Where(x => x.GroupName.CourseNumber == courseNumber).ToList();
    }

    public void ChangeStudentGroup(Student student, Group newGroup)
    {
        var oldGroup = FindGroup(student.GroupName) ??
                       throw GroupNotExistException.GroupNotExist(student.GroupName);

        GetStudent(student.Id); // проверка, есть ли студент в ису
        oldGroup.DeleteStudent(student);
        student.ChangeGroup(newGroup);
        newGroup.AddStudent(student);
    }
}