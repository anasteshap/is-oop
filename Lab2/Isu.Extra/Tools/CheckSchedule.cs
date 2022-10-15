using Isu.Extra.Entities;
using Isu.Extra.Models;
namespace Isu.Extra.Tools;

public class CheckSchedule
{
    public static void CheckValidLessonTime(BaseTime timeOfFirstPossibleLesson, BaseTime timeOfLastPossibleLesson, Lesson lesson)
    {
        if (lesson is null)
        {
            throw new Exception("Null");
        }

        if (lesson.LessonTime.StartTime.Minutes != 0)
        {
            throw new Exception();
        }

        if (lesson.LessonTime.StartTime.Hour < timeOfFirstPossibleLesson.Hour
            && lesson.LessonTime.StartTime.Hour > timeOfLastPossibleLesson.Hour)
        {
            throw new Exception();
        }

        if ((lesson.LessonTime.StartTime.Hour - timeOfFirstPossibleLesson.Hour) % 2 != 0)
        {
            throw new Exception();
        }
    }

    public static void CheckLessonCrossing(List<Lesson> evenWeek, List<Lesson> oddWeek, Lesson lesson)
    {
        if (lesson.IsWeekEven)
        {
            var a = evenWeek.FirstOrDefault(x => Equals(x.LessonTime, lesson.LessonTime));
            if (a != null)
            {
                throw new Exception($"{a.LessonTime.StartTime.Hour}, {lesson.Name}");
            }
        }
        else
        {
            if (oddWeek.FirstOrDefault(x => x.LessonTime.Equals(lesson.LessonTime)) != null)
            {
                throw new Exception();
            }
        }
    }

    public static bool IsLessonCrossing(Schedule schedule, Lesson lesson)
    {
        /*if (schedule.LessonsOfEvenWeek[1].LessonTime == lesson.LessonTime)
        {
            throw new Exception($"{lesson.LessonTime.StartTime.Hour}");
        }*/

        if (lesson.IsWeekEven)
        {
            var a = schedule.LessonsOfEvenWeek.FirstOrDefault(x => Equals(lesson.LessonTime, x.LessonTime));
            if (a != null)
            {
                Console.WriteLine($"{a.LessonTime.StartTime.Hour} {lesson.LessonTime.StartTime.Hour}");
                return true;
            }
        }
        else
        {
            if (schedule.LessonsOfOddWeek.FirstOrDefault(x => Equals(lesson.LessonTime, x.LessonTime)) != null)
            {
                return true;
            }
        }

        return false;
    }
}