using Backups.Algorithms;
using Backups.Archivers;
using Backups.Entities;
using Backups.Repository;

namespace Backups.Interfaces;

public interface IBackupTask
{
    string Name { get; }
    IRepository Repository { get; }
    IAlgorithm Algorithm { get; }
    IArchiver Archiver { get; }
    IReadOnlyCollection<IBackupObject> BackupObjects();
    IReadOnlyCollection<RestorePoint> RestorePoints();
    void AddBackupObject(IBackupObject backupObject);
    void RemoveBackupObject(IBackupObject backupObject);
    RestorePoint Working();
}