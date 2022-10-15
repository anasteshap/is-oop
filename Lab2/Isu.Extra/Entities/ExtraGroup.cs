using Isu.Entities;
using Isu.Extra.Exceptions;
using Isu.Models;

namespace Isu.Extra.Entities;

public class ExtraGroup
{
    private readonly Group _group;

    public ExtraGroup(Group group, Schedule schedule)
    {
        _group = group;
        Schedule = schedule;
    }

    public Schedule Schedule { get; private set; }
    public char FacultyName { get => _group.GroupName.Faculty; }
    public GroupName GroupName { get => _group.GroupName; }

    public IReadOnlyCollection<Student> Students { get => _group.Students; }

    public void ChangeSchedule(Schedule newSchedule)
    {
        if (newSchedule is null)
        {
            throw ExtraGroupException.InvalidSchedule();
        }

        Schedule = newSchedule;
    }
}