using Isu.Extra.Models;

namespace Isu.Extra.Entities;

public class Ognp : IEquatable<Ognp>
{
    private readonly List<OgnpStream> _streams;
    private readonly List<OgnpGroup> _allOgnpGroups = new ();

    public Ognp(OgnpName ognpName, List<OgnpStream> streams)
    {
        if (streams is null)
        {
            throw new Exception();
        }

        Name = ognpName;
        _streams = streams;
        _allOgnpGroups = streams.SelectMany(x => x.Lessons().Values.ToList()).ToList();
    }

    public OgnpName Name { get; }

    public static void AddStream(Ognp ognp, OgnpStream ognpStream)
    {
        if (ognpStream is null)
        {
            throw new Exception();
        }

        if (ognp._streams.Contains(ognpStream))
        {
            throw new Exception();
        }

        ognp._streams.Add(ognpStream);
        ognp._allOgnpGroups.AddRange(ognpStream.Lessons().Values.ToList());
    }

    public IReadOnlyList<OgnpStream> Streams() => _streams.AsReadOnly();
    public IReadOnlyList<OgnpGroup> OgnpGroups() => _allOgnpGroups.AsReadOnly();

    public OgnpGroupName AddStudent(ExtraStudent extraStudent, int numOgnpStream)
    {
        return GetOgnpStream(numOgnpStream).AddStudent(extraStudent);
    }

    public void RemoveStudent(ExtraStudent extraStudent)
    {
        GetStudentStream(extraStudent).RemoveStudent(extraStudent);
    }

    public OgnpStream? FindStudentStream(ExtraStudent extraStudent)
    {
        return _streams.FirstOrDefault(x => x.ExtraStudents().Contains(extraStudent));
    }

    public OgnpStream GetStudentStream(ExtraStudent extraStudent)
    {
        return FindStudentStream(extraStudent) ?? throw new Exception();
    }

    public OgnpStream GetOgnpStream(int numOgnpStream)
    {
        return _streams.FirstOrDefault(x => x.Number == numOgnpStream) ?? throw new Exception();
    }

    public bool Equals(Ognp? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _allOgnpGroups.Equals(other._allOgnpGroups) && Name.Equals(other.Name);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Ognp)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_allOgnpGroups, Name);
    }
}