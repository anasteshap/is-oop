namespace Isu.Extra.Models;

public class LessonTime : IEquatable<LessonTime>
{
    public LessonTime(DayOfWeek dayOfWeek, BaseTime startTime)
    {
        if (startTime is null)
        {
            throw new Exception();
        }

        DayOfWeek = dayOfWeek;
        StartTime = startTime;
        uint minutes = (startTime.Hour * 60) + startTime.Minutes + 90;
        EndTime = new BaseTime(minutes / 60, minutes % 60);
    }

    public DayOfWeek DayOfWeek { get; }
    public BaseTime StartTime { get; }
    public BaseTime EndTime { get; }

    public override int GetHashCode()
    {
        return HashCode.Combine((int)DayOfWeek, StartTime);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((LessonTime)obj);
    }

    public bool Equals(LessonTime? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return DayOfWeek == other.DayOfWeek && StartTime.Equals(other.StartTime);
    }
}