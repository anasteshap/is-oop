using Isu.Extra.Models;
using Isu.Extra.Tools;

namespace Isu.Extra.Entities;

public class Schedule
{
    private Schedule(List<Lesson> evenWeek, List<Lesson> oddWeek)
    {
        LessonsOfEvenWeek = evenWeek;
        LessonsOfOddWeek = oddWeek;
    }

    public static ScheduleBuilder Builder => new ScheduleBuilder();
    public IReadOnlyList<Lesson> LessonsOfEvenWeek { get; }
    public IReadOnlyList<Lesson> LessonsOfOddWeek { get; }

    public static Schedule operator +(Schedule schedule1, Schedule schedule2)
    {
        var newListOfLessons = schedule1.LessonsOfEvenWeek
            .Concat(schedule1.LessonsOfOddWeek)
            .Concat(schedule2.LessonsOfEvenWeek)
            .Concat(schedule2.LessonsOfOddWeek)
            .ToList();
        return Builder.AddLessonsList(newListOfLessons).Build();
    }

    public class ScheduleBuilder
    {
        private readonly BaseTime _timeOfFirstPossibleLesson = new (10, 0);
        private readonly BaseTime _timeOfLastPossibleLesson = new (18, 0);
        private readonly List<Lesson> _evenWeek = new ();
        private readonly List<Lesson> _oddWeek = new ();

        public ScheduleBuilder AddLessonsList(List<Lesson> lessons)
        {
            if (lessons.Count != 0)
            {
                lessons.ForEach(x => AddOneLesson(x));
            }

            return this;
        }

        public Schedule Build()
        {
            return new Schedule(_evenWeek, _oddWeek);
        }

        private Lesson AddOneLesson(Lesson lesson)
        {
            CheckSchedule.CheckValidLessonTime(_timeOfFirstPossibleLesson, _timeOfLastPossibleLesson, lesson);
            CheckSchedule.CheckLessonCrossing(_evenWeek, _oddWeek, lesson);

            if (lesson.IsWeekEven)
            {
                _evenWeek.Add(lesson);
            }
            else
            {
                _oddWeek.Add(lesson);
            }

            return lesson;
        }
    }
}
