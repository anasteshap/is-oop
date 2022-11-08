using Backups.Inter;
using Backups.Repository;

namespace Backups.Entities;

public class RestorePoint
{
    private readonly List<IBackupObject> _backupObjects;
    public RestorePoint(string path, IRepository repository, List<IBackupObject> backupObjects)
    {
        FullName = Path.GetFullPath(path);
        Repository = repository;
        CreationDate = DateTime.Now;
        _backupObjects = backupObjects;
    }

    public string FullName { get; }
    public IRepository Repository { get; }
    public DateTime CreationDate { get; }
    public IReadOnlyCollection<IBackupObject> BackupObjects() => _backupObjects;
}