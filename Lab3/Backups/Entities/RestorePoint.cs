using Backups.Exceptions;
using Backups.Interfaces;
using Backups.Repository;
using Backups.Storage;

namespace Backups.Entities;

public class RestorePoint
{
    private readonly List<IBackupObject> _backupObjects;
    public RestorePoint(string relativePath, DateTime dateTime, IStorage storage, List<IBackupObject> backupObjects)
    {
        if (string.IsNullOrEmpty(relativePath))
        {
            throw NullException.InvalidName();
        }

        RelativePath = relativePath;
        Storage = storage;
        CreationDate = dateTime;
        _backupObjects = backupObjects;
    }

    public string RelativePath { get; }
    public IStorage Storage { get; }
    public DateTime CreationDate { get; }
    public IReadOnlyCollection<IBackupObject> BackupObjects() => _backupObjects;
}