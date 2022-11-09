using Backups.Interfaces;
using Backups.Repository;
using Backups.Storage;

namespace Backups.Entities;

public class RestorePoint
{
    private readonly List<IBackupObject> _backupObjects;
    public RestorePoint(string name, DateTime dateTime, IStorage storage, List<IBackupObject> backupObjects)
    {
        Name = name;
        Storage = storage;
        CreationDate = dateTime;
        _backupObjects = backupObjects;
    }

    public string Name { get; }
    public IStorage Storage { get; }
    public DateTime CreationDate { get; }
    public IReadOnlyCollection<IBackupObject> BackupObjects() => _backupObjects;
}