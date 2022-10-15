using Isu.Extra.Models;

namespace Isu.Extra.Entities;

public class Lesson
{
    private const int MaxCountOfStudents = 20;

    public Lesson(string name, string teacherName, uint audienceRoom, LessonTime lessonTime, bool isWeekEven)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new Exception();
        }

        Name = name;
        Teacher = new Teacher(teacherName);
        Audience = new Audience(audienceRoom);
        LessonTime = lessonTime;
        IsWeekEven = isWeekEven;
        Id = Guid.NewGuid();
    }

    public string Name { get; }
    public Teacher Teacher { get; }
    public Audience Audience { get; }
    public LessonTime LessonTime { get; }
    public bool IsWeekEven { get; }
    public Guid Id { get; }

    public static bool IsPossibleToAddStudent(OgnpGroup ognpGroup)
    {
        return ognpGroup.Students().Count < MaxCountOfStudents;
    }
}