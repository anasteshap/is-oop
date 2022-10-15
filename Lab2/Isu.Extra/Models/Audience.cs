using Isu.Extra.Exceptions;

namespace Isu.Extra.Models;

public class Audience
{
    public Audience(uint room)
    {
        if (room < 1)
        {
            throw LessonException.InvalidAudience();
        }

        Room = room;
    }

    public uint Room { get; }
}