namespace Backups.Exceptions;

public class BackupException : Exception
{
    private BackupException(string message)
        : base(message) { }

    public static BackupException BackupObjectDoesNotExist(string path)
    {
        return new BackupException($"BackupObject {path} doesn't exist");
    }

    public static BackupException BackupObjectAlreadyExists(string path)
    {
        return new BackupException($"BackupObject {path} already exists");
    }

    public static BackupException RestorePointDoesNotExist(string path)
    {
        return new BackupException($"RestorePoint {path} doesn't exist");
    }

    public static BackupException RestorePointAlreadyExists(string path)
    {
        return new BackupException($"RestorePoint {path} already exists");
    }
}