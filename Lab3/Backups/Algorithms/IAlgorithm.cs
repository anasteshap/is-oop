using Backups.Archivers;
using Backups.Entities;
using Backups.Interfaces;
using Backups.Repository;
using Backups.Storage;

namespace Backups.Algorithms;

public interface IAlgorithm
{
    IStorage Save(IRepository repository, IArchiver archiver, IReadOnlyCollection<IBackupObject> backupObjects, string fullPathOfRestorePoint);
}