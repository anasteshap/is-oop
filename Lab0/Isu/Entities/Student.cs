using Isu.Models;

namespace Isu.Entities;

public class Student
{
    public Student(string name, int id, GroupName groupName)
    {
        Name = name;
        Id = id;
        GroupName = groupName;
    }

    public int Id { get; }
    public string Name { get; }
    public GroupName GroupName { get; set; }
}