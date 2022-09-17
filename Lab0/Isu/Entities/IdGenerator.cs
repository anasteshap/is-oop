namespace Isu.Entities;

public class IdGenerator
{
    private int _id = 0;

    public int NewId()
    {
        _id++;
        return _id;
    }
}