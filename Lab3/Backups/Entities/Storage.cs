namespace Backups.Entities;

public class Storage
{
    public Storage(string fullName)
    {
        if (string.IsNullOrEmpty(fullName))
        {
            throw new Exception();
        }

        FullName = fullName;
    }

    public string FullName { get; }
}