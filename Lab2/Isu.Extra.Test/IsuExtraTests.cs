using Isu.Entities;
using Isu.Extra.Entities;
using Isu.Extra.Exceptions;
using Isu.Extra.Models;
using Isu.Extra.Services;
using Isu.Models;
using Xunit;
namespace Isu.Extra.Test;

public class IsuExtraTests
{
    [Fact]
    public static void RegisterStudentOnOgnp_ThrowException()
    {
        var mf1 = new MegaFaculty("TINT", new List<char>() { 'M', 'K' });
        var mf2 = new MegaFaculty("KTU", new List<char>() { 'L', 'G' });
        var isuExtraService = new IsuExtraService(new List<MegaFaculty>() { mf1, mf2 });

        var mainLesson = new Lesson(
            ".10",
            "Teacher",
            1404,
            new LessonTime(DayOfWeek.Monday, new BaseTime(12, 0)),
            true);
        var groupSchedule = Schedule.Builder
            .AddLessonsList(new List<Lesson>() { mainLesson })
            .Build();
        var group = isuExtraService.AddGroup(new GroupName("M32021", new CourseNumber(2)));
        isuExtraService.AddScheduleToGroup(group, groupSchedule);

        var ognp11 = new Ognp(new OgnpName("Math1", "TINT"), new List<OgnpStream>());
        var ognp12 = new Ognp(new OgnpName("Rus1", "TINT"), new List<OgnpStream>());
        var ognp21 = new Ognp(new OgnpName("Math2", "KTU"), new List<OgnpStream>());
        var ognp22 = new Ognp(new OgnpName("Rus2", "KTU"), new List<OgnpStream>());
        isuExtraService.AddOgnp(ognp11);
        isuExtraService.AddOgnp(ognp12);
        isuExtraService.AddOgnp(ognp21);
        isuExtraService.AddOgnp(ognp22);

        var lesson1 = new Lesson(
            ".1",
            "Elena Nik",
            331,
            new LessonTime(DayOfWeek.Monday, new BaseTime(12, 0)),
            true);

        var lesson2 = new Lesson(
            ".2",
            "Marina Vlad",
            302,
            new LessonTime(DayOfWeek.Tuesday, new BaseTime(12, 0)),
            true);

        var lesson3 = new Lesson(
            ".3",
            "Anna Serg",
            466,
            new LessonTime(DayOfWeek.Wednesday, new BaseTime(12, 0)),
            true);
        isuExtraService.AddStreamOgnp(ognp11, 1, new List<Lesson>() { lesson3 });
        isuExtraService.AddStreamOgnp(ognp21, 2, new List<Lesson>() { lesson1 });
        isuExtraService.AddStreamOgnp(ognp22, 3, new List<Lesson>() { lesson2 });

        var student = isuExtraService.AddStudent(group, "Anastasiia");
        var newStudent = isuExtraService.AddStudent(group, "Dasha");

        Assert.Throws<MegaFacultyException>(() => isuExtraService.AddStudentOnOgnpStream(student, ognp11, 1));
        Assert.Throws<LessonException>(() => isuExtraService.AddStudentOnOgnpStream(student, ognp21, 2));
        var studentOgnpGroupName = isuExtraService.AddStudentOnOgnpStream(student, ognp22, 3);

        Assert.DoesNotContain(lesson1, isuExtraService.GetScheduleByStudent(student).LessonsOfEvenWeek);
        Assert.Contains(mainLesson, isuExtraService.GetScheduleByStudent(student).LessonsOfEvenWeek);

        Assert.DoesNotContain(student, isuExtraService.FindStudentsByOgnp(ognp21));
        Assert.Contains(student, isuExtraService.FindStudentsByOgnp(ognp22));
        Assert.True(isuExtraService.IsStudentHaveOgnp(student, ognp22.Name));

        Assert.Contains(newStudent, isuExtraService.GetStudentsByGroupWithoutOgnp(group.GroupName));
        Assert.DoesNotContain(student, isuExtraService.GetStudentsByGroupWithoutOgnp(group.GroupName));

        var studentsByOgnpGroup = isuExtraService.GetStudentsByOgnpGroup(studentOgnpGroupName);
        Assert.Contains(student, studentsByOgnpGroup);

        isuExtraService.RemoveStudentByOgnp(student, ognp22);
        Assert.DoesNotContain(lesson2, isuExtraService.GetScheduleByStudent(student).LessonsOfEvenWeek);
        Assert.DoesNotContain(student, isuExtraService.FindStudentsByOgnp(ognp22));
    }
}