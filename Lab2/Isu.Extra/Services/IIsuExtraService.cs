using Isu.Entities;
using Isu.Extra.Entities;
using Isu.Extra.Models;
using Isu.Models;
using Isu.Services;

namespace Isu.Extra.Services;

public interface IIsuExtraService : IIsuService
{
    void AddScheduleToGroup(Group group, Schedule schedule);
    void AddOgnp(Ognp ognp);
    OgnpGroupName AddStudentOnOgnpStream(Student student, Ognp ognp, int numOgnpStream);
    void RemoveStudentByOgnp(Student student, Ognp ognp);
    Schedule GetScheduleByGroup(Group group);
    Schedule GetScheduleByStudent(Student student);
    List<Student> GetStudentsByGroupWithoutOgnp(GroupName groupName);
    List<Student> GetStudentsByOgnpGroup(OgnpGroupName ognpGroupName);
    List<OgnpStream> GetOgnpStreams(OgnpName ognpName);
    OgnpStream AddStreamOgnp(Ognp ognp, int number, List<Lesson> lessons);
    List<Student> FindStudentsByOgnp(Ognp ognp);
    bool IsStudentHaveOgnp(Student student, OgnpName ognpName);
}