namespace Isu.Entities;

public class IdGenerator
{
    private static int _id;

    public IdGenerator()
    {
        _id = 0;
    }

    public int Id
    {
        get
        {
            ++_id;
            return _id;
        }
    }
}