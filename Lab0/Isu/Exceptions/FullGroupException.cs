namespace Isu.Exceptions;

public class FullGroupException : Exception
{
    public FullGroupException(string? message = "Group is full")
        : base(message) { }
}