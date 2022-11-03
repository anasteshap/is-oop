using Backups.Algorithms;
using Backups.Entities;
using Backups.Repository;

namespace Backups.Inter;

public interface IBackupTask
{
    string Name { get; }
    IRepository Repository { get; }
    IAlgorithm Algorithm { get; }
    IBackup Backup { get; }
    IReadOnlyCollection<IBackupObject> BackupObjects();
    void AddBackupObject(IBackupObject backupObject);
    void RemoveBackupObject(IBackupObject backupObject);
    RestorePoint Working();
}