namespace Isu.Extra.Models;

public class BaseTime : IEquatable<BaseTime>
{
    public BaseTime(uint hour, uint minutes)
    {
        Hour = hour;
        Minutes = minutes;
    }

    public uint Hour { get; }
    public uint Minutes { get; }

    public bool Equals(BaseTime? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Hour == other.Hour && Minutes == other.Minutes;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((BaseTime)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Hour, Minutes);
    }
}