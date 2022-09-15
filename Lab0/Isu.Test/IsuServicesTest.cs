using Isu.Entities;
using Isu.Exceptions;
using Isu.Models;
using Isu.Services;
using Xunit;

namespace Isu.Test;

public class IsuServicesTest
{
    [Fact]
    public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
    {
        var isuService = new IsuService();
        var groupName = new GroupName("M33031", new CourseNumber(3));
        Group group = isuService.AddGroup(groupName);
        Student student1 = isuService.AddStudent(group, "Anastasiia Pinchuk");
        Student student2 = isuService.AddStudent(group, "Daria Plekhanova");

        Assert.True(student1.GroupName == group.GroupName);
        Assert.Contains(student1, group.Students);
        Assert.Contains(student2, group.Students);
    }

    [Fact]
    public void ReachMaxStudentPerGroup_ThrowException()
    {
        var isuService = new IsuService();
        var group = isuService.AddGroup(new GroupName("M32021", new CourseNumber(2)));

        Assert.Throws<FullGroupException>(() =>
        {
            for (int i = 0; i < 24; ++i)
            {
                isuService.AddStudent(group, $"Person {i + 1}");
            }
        });
    }

    [Fact]
    public void CreateGroupWithInvalidName_ThrowException()
    {
        Assert.Throws<GroupNameException>(() => new GroupName("M333031", new CourseNumber(3)));
    }

    [Fact]
    public void TransferStudentToAnotherGroup_GroupChanged()
    {
        var isuService = new IsuService();
        var oldGroup = isuService.AddGroup(new GroupName("M32021", new CourseNumber(2)));
        var newGroup = isuService.AddGroup(new GroupName("M32001", new CourseNumber(2)));

        var student = isuService.AddStudent(oldGroup, "Anastasiia Pinchuk");
        isuService.ChangeStudentGroup(student, newGroup);

        Assert.True(student.GroupName == newGroup.GroupName);
        Assert.True(!oldGroup.StudentInGroup(student));
        Assert.True(newGroup.StudentInGroup(student));
    }
}