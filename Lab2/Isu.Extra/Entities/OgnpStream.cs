using Isu.Extra.Exceptions;
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
            throw LessonException.InvalidName();
        }

        if (_lessons.ContainsKey(lesson))
        {
            throw OgnpException.LessonAlreadyExist(this, lesson);
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
            throw ExtraStudentException.InvalidStudent();
        }

        if (_extraStudents.Contains(extraStudent))
        {
            throw ExtraStudentException.StudentAlreadyExist();
        }

        var studentSchedule = extraStudent.ExtraGroup.Schedule + extraStudent.OgnpSchedule();
        var possibleLessons = _lessons.Keys
            .Where(lesson => !CheckSchedule.IsLessonCrossing(studentSchedule, lesson))
            .ToList();
        var resultLesson = possibleLessons
            .FirstOrDefault(lesson => Lesson.IsPossibleToAddStudent(_lessons[lesson])) ?? throw LessonException.InvalidName();

        _extraStudents.Add(extraStudent);
        _lessons[resultLesson].AddStudent(extraStudent);
        return _lessons[resultLesson].GroupName;
    }

    public void RemoveStudent(ExtraStudent extraStudent)
    {
        if (extraStudent is null)
        {
            throw ExtraStudentException.InvalidStudent();
        }

        if (!_extraStudents.Remove(extraStudent))
        {
            throw ExtraStudentException.InvalidStudent();
        }

        _lessons.Values.First(x => x.Students().Contains(extraStudent)).RemoveStudent(extraStudent);
    }

    public Lesson FindLessonByStudent(ExtraStudent extraStudent)
    {
        return _lessons.FirstOrDefault(x => x.Value.Students().Contains(extraStudent)).Key;
    }
}