using Isu.Extra.Models;
namespace Isu.Extra.Entities;

public class OgnpGroup
{
    private const int MaxCountOfPeople = 23;
    private readonly List<ExtraStudent> _students;

    public OgnpGroup(OgnpGroupName groupName)
    {
        GroupName = groupName;
        _students = new List<ExtraStudent>();
    }

    public OgnpGroupName GroupName { get; }
    public IReadOnlyList<ExtraStudent> Students() => _students.AsReadOnly();

    public void AddStudent(ExtraStudent student)
    {
        if (_students.Count == MaxCountOfPeople)
        {
            throw new Exception();
        }

        if (_students.Contains(student))
        {
            throw new Exception();
        }

        _students.Add(student);
    }

    public void RemoveStudent(ExtraStudent student)
    {
        if (!_students.Remove(student))
        {
            throw new Exception();
        }
    }

    /*public bool StudentInGroup(ExtraStudent student)
    {
        return _students.Contains(student);
    }*/
}