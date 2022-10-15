using Isu.Entities;
using Isu.Extra.Models;
using Isu.Models;

namespace Isu.Extra.Entities;

public class ExtraStudent
{
    private const int MaxCountOfOgnp = 2;
    private readonly List<Ognp> _ognps = new ();
    private readonly Student _student;

    public ExtraStudent(Student student, ExtraGroup extraGroup)
    {
        _student = student;
        ExtraGroup = extraGroup;
    }

    public ExtraGroup ExtraGroup { get; private set; }
    public int Id { get => _student.Id; }
    public string Name { get => _student.Name; }

    public static void StudentAddOgnp(ExtraStudent extraStudent, Ognp ognp)
    {
        if (!IsPossibleToAddOgnp(extraStudent))
        {
            throw new Exception();
        }

        if (extraStudent._ognps.Count == 1)
        {
            if (extraStudent._ognps[0].Name.MegaFaculty != ognp.Name.MegaFaculty)
            {
                throw new Exception();
            }
        }

        extraStudent._ognps.Add(ognp);
    }

    public static void StudentRemoveOgnp(ExtraStudent extraStudent, Ognp ognp)
    {
        if (!extraStudent._ognps.Remove(ognp))
        {
            throw new Exception();
        }
    }

    public Schedule OgnpSchedule() => Schedule.Builder.AddLessonsList(GetScheduleOgnp()).Build();

    public void ChangeGroup(ExtraGroup extraGroup)
    {
        ExtraGroup = extraGroup;
        var newGroup = new Group(extraGroup.GroupName);
        _student.ChangeGroup(newGroup);
    }

    public List<Lesson> GetScheduleOgnp()
    {
        var listGroupsOfLessons = _ognps
            .SelectMany(x => x.Streams().ToList().SelectMany(y => y.Lessons()))
            .ToList();
        return listGroupsOfLessons
            .Select(x => x.Key)
            .ToList()
            .Where(x => listGroupsOfLessons.Exists(y => y.Value.Students().Contains(this)))
            .ToList();
    }

    public bool IsEnrolledOnOgnp()
    {
        return _ognps.Count != 0;
    }

    public bool IsHaveConcreteOgnp(OgnpName ognpName)
    {
        if (_ognps.FirstOrDefault(x => x.Name == ognpName) == null)
        {
            return false;
        }

        return true;
    }

    private static bool IsPossibleToAddOgnp(ExtraStudent extraStudent)
    {
        return extraStudent._ognps.Count < MaxCountOfOgnp;
    }
}