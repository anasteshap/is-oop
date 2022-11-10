using Backups.Interfaces;
using Backups.Repository;
using Backups.Storage;

namespace Backups.Entities;

public class RestorePoint
{
    private readonly List<IBackupObject> _backupObjects;
    public RestorePoint(string name, DateTime dateTime, SplitStorage storage, List<IBackupObject> backupObjects)
    {
        Name = name;
        Storage = storage;
        CreationDate = dateTime;
        _backupObjects = backupObjects;
    }

    public string Name { get; }
    public SplitStorage Storage { get; }
    public DateTime CreationDate { get; }
    public IReadOnlyCollection<IBackupObject> BackupObjects() => _backupObjects;
}