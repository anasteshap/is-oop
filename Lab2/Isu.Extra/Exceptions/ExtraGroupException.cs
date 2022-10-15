namespace Isu.Extra.Exceptions;

public class ExtraGroupException : Exception
{
    private ExtraGroupException(string message)
        : base(message) { }

    public static ExtraGroupException InvalidSchedule()
    {
        return new ExtraGroupException("Invalid data");
    }

    public static ExtraGroupException GroupIsFull()
    {
        return new ExtraGroupException("Group is full");
    }
}