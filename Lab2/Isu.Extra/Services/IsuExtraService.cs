using Isu.Entities;
using Isu.Extra.Entities;
using Isu.Extra.Exceptions;
using Isu.Extra.Models;
using Isu.Models;
using Isu.Services;
using Isu.Test;

namespace Isu.Extra.Services;

public class IsuExtraService : IIsuExtraService
{
    private readonly IIsuService _isuService;
    private readonly List<MegaFaculty> _megaFaculties;
    private readonly List<Ognp> _ognps = new ();
    private readonly Dictionary<Student, ExtraStudent> _extraStudents = new ();
    private readonly Dictionary<Group, ExtraGroup> _extraGroups = new ();

    public IsuExtraService(List<MegaFaculty> megaFaculties)
    {
        _megaFaculties = megaFaculties;
        _isuService = new IsuService();
    }

    public Group AddGroup(GroupName name)
    {
        var groupMegaFaculty = _megaFaculties.FirstOrDefault(x => x.Faculties().Contains(name.Faculty));
        if (groupMegaFaculty is null)
        {
            throw MegaFacultyException.InvalidName();
        }

        var group = _isuService.AddGroup(name);
        var extraGroup = new ExtraGroup(group, Schedule.Builder.Build());
        _extraGroups[group] = extraGroup;
        return group;
    }

    public Student AddStudent(Group group, string name)
    {
        var student = _isuService.AddStudent(group, name);
        var extraStudent = new ExtraStudent(student, _extraGroups[group]);
        _extraStudents[student] = extraStudent;
        return student;
    }

    public Student GetStudent(int id)
    {
        return _isuService.GetStudent(id);
    }

    public Student? FindStudent(int id)
    {
        return _isuService.FindStudent(id);
    }

    public List<Student> FindStudents(GroupName groupName)
    {
        return _isuService.FindStudents(groupName);
    }

    public List<Student> FindStudents(CourseNumber courseNumber)
    {
        return _isuService.FindStudents(courseNumber);
    }

    public Group? FindGroup(GroupName groupName)
    {
        return _isuService.FindGroup(groupName);
    }

    public List<Group> FindGroups(CourseNumber courseNumber)
    {
        return _isuService.FindGroups(courseNumber);
    }

    public void ChangeStudentGroup(Student student, Group newGroup)
    {
        _isuService.ChangeStudentGroup(student, newGroup);
        _extraStudents[student].ChangeGroup(_extraGroups[newGroup]);
    }

    public void AddScheduleToGroup(Group group, Schedule schedule)
    {
        _extraGroups[group].ChangeSchedule(schedule);
    }

    public void AddOgnp(Ognp ognp)
    {
        if (_ognps.Contains(ognp))
        {
            throw OgnpException.OgnpAlreadyExist();
        }

        var megaFaculty = _megaFaculties.FirstOrDefault(x => Equals(x.Name, ognp.Name.MegaFaculty));
        if (megaFaculty is null)
        {
            throw MegaFacultyException.InvalidName();
        }

        _ognps.Add(ognp);
    }

    public OgnpGroupName AddStudentOnOgnpStream(Student student, Ognp ognp, int numOgnpStream)
    {
        if (!_ognps.Contains(ognp) || !_extraStudents.Keys.ToList().Contains(student) || numOgnpStream < 1)
        {
            throw OgnpException.InvalidName();
        }

        if (Equals(ognp.Name.MegaFaculty, GetStudentMegaFacultyName(student)))
        {
            throw MegaFacultyException.InvalidName();
        }

        var extraStudent = _extraStudents[student];
        var ognpGroupName = ognp.AddStudent(extraStudent, numOgnpStream);
        ExtraStudent.StudentAddOgnp(extraStudent, ognp);
        return ognpGroupName;
    }

    public void RemoveStudentByOgnp(Student student, Ognp ognp)
    {
        if (!_ognps.Contains(ognp) || !_extraStudents.Keys.ToList().Contains(student))
        {
            throw OgnpException.InvalidName();
        }

        var extraStudent = _extraStudents[student];
        ExtraStudent.StudentRemoveOgnp(extraStudent, ognp);
        ognp.RemoveStudent(extraStudent);
    }

    public Schedule GetScheduleByGroup(Group group)
    {
        return _extraGroups[group].Schedule;
    }

    public Schedule GetScheduleByStudent(Student student)
    {
        var extraStudent = _extraStudents[student];
        return extraStudent.ExtraGroup.Schedule + extraStudent.OgnpSchedule();
    }

    public List<Student> GetStudentsByGroupWithoutOgnp(GroupName groupName)
    {
        return FindStudents(groupName).Where(x => !_extraStudents[x].IsEnrolledOnOgnp()).ToList();
    }

    public List<Student> GetStudentsByOgnpGroup(OgnpGroupName ognpGroupName)
    {
        var extraStudents = FindOgnpGroup(ognpGroupName)?.Students().ToList() ?? throw new Exception();
        var students = extraStudents.Select(x => ExtraStudentToStudent(x)).ToList();
        return students;
    }

    public List<OgnpStream> GetOgnpStreams(OgnpName ognpName)
    {
        var ognp = _ognps.FirstOrDefault(x => Equals(x.Name, ognpName));
        return ognp?.Streams().ToList() ?? new List<OgnpStream>();
    }

    public OgnpStream AddStreamOgnp(Ognp ognp, int number, List<Lesson> lessons)
    {
        if (!_ognps.Contains(ognp))
        {
            throw OgnpException.InvalidName();
        }

        var stream = new OgnpStream(number, lessons);
        Ognp.AddStream(ognp, stream);
        return stream;
    }

    public List<Student> FindStudentsByOgnp(Ognp ognp)
    {
        var tempOgnp = _ognps.FirstOrDefault(x => Equals(x, ognp));
        if (tempOgnp is null)
        {
            return new List<Student>();
        }

        var extraStudents = tempOgnp.Streams().SelectMany(x => x.ExtraStudents());
        return extraStudents.Select(x => ExtraStudentToStudent(x)).ToList();
    }

    public bool IsStudentHaveOgnp(Student student, OgnpName ognpName)
    {
        return _extraStudents[student].IsHaveConcreteOgnp(ognpName);
    }

    private OgnpGroup? FindOgnpGroup(OgnpGroupName ognpGroupName)
    {
        var groupsOfAllOgnps = _ognps.SelectMany(x => x.OgnpGroups()).ToList();
        return groupsOfAllOgnps.FirstOrDefault(x => x.GroupName == ognpGroupName);
    }

    private Student ExtraStudentToStudent(ExtraStudent extraStudent)
    {
        return GetStudent(extraStudent.Id);
    }

    private string GetStudentMegaFacultyName(Student student)
    {
        if (!_extraStudents.Keys.ToList().Contains(student))
        {
            throw ExtraStudentException.InvalidStudent();
        }

        var studentFaculty = student.GroupName.Faculty;
        return _megaFaculties.First(x => x.Faculties().Contains(studentFaculty)).Name;
    }
}