namespace Isu.Extra.Models;

public class Audience
{
    public Audience(uint room)
    {
        if (room < 1)
        {
            throw new Exception();
        }

        Room = room;
    }

    public uint Room { get; }
}