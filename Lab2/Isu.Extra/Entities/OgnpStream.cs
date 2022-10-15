using Isu.Extra.Models;
using Isu.Extra.Tools;

namespace Isu.Extra.Entities;

public class OgnpStream
{
    private const int MaxCountOfStudents = 100;

    private readonly List<ExtraStudent> _extraStudents = new ();
    private readonly Dictionary<Lesson, OgnpGroup> _lessons = new ();

    public OgnpStream(int number, List<Lesson> lessons)
    {
        lessons.ForEach(x => _lessons[x] = new OgnpGroup(new OgnpGroupName(x.Name)));
        Number = number;
        Schedule = Schedule.Builder.AddLessonsList(lessons).Build();
    }

    public int Number { get; }
    public Schedule Schedule { get; private set; }
    public IReadOnlyDictionary<Lesson, OgnpGroup> Lessons() => _lessons;
    public IReadOnlyList<ExtraStudent> ExtraStudents() => _extraStudents.AsReadOnly();

    public void AddLesson(Lesson lesson)
    {
        if (lesson is null)
        {
            throw new Exception();
        }

        if (_lessons.ContainsKey(lesson))
        {
            throw new Exception();
        }

        _lessons[lesson] = new OgnpGroup(new OgnpGroupName(lesson.Name));
        Schedule = Schedule.Builder
            .AddLessonsList(_lessons.Keys.ToList())
            .Build();
    }

    public OgnpGroupName AddStudent(ExtraStudent extraStudent)
    {
        if (extraStudent is null)
        {
            throw new Exception();
        }

        if (_extraStudents.Contains(extraStudent))
        {
            throw new Exception();
        }

        var studentSchedule = extraStudent.ExtraGroup.Schedule + extraStudent.OgnpSchedule();
        var possibleLessons = _lessons.Keys
            .Where(lesson => !CheckSchedule.IsLessonCrossing(studentSchedule, lesson))
            .ToList();
        var resultLesson = possibleLessons
            .FirstOrDefault(lesson => Lesson.IsPossibleToAddStudent(_lessons[lesson])) ?? throw new Exception();

        _extraStudents.Add(extraStudent);
        _lessons[resultLesson].AddStudent(extraStudent);
        return _lessons[resultLesson].GroupName;
    }

    public void RemoveStudent(ExtraStudent extraStudent)
    {
        if (extraStudent is null)
        {
            throw new Exception();
        }

        if (!_extraStudents.Remove(extraStudent))
        {
            throw new Exception();
        }

        _lessons.Values.First(x => x.Students().Contains(extraStudent)).RemoveStudent(extraStudent);
    }

    public Lesson FindLessonByStudent(ExtraStudent extraStudent)
    {
        return _lessons.FirstOrDefault(x => x.Value.Students().Contains(extraStudent)).Key;
    }
}