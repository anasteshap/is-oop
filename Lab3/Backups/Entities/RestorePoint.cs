using Backups.Interfaces;
using Backups.Repository;

namespace Backups.Entities;

public class RestorePoint
{
    private readonly List<IBackupObject> _backupObjects;
    public RestorePoint(string relativePath, IRepository repository, List<IBackupObject> backupObjects)
    {
        RelativePath = Path.GetFullPath(relativePath);
        Repository = repository;
        CreationDate = DateTime.Now;
        _backupObjects = backupObjects;
    }

    public string RelativePath { get; }
    public IRepository Repository { get; }
    public DateTime CreationDate { get; }
    public IReadOnlyCollection<IBackupObject> BackupObjects() => _backupObjects;
}