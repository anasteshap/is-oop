using Backups.Archivers;
using Backups.Entities;
using Backups.Interfaces;
using Backups.Repository;
using Backups.Storage;

namespace Backups.Algorithms;

public interface IAlgorithm
{
    SplitStorage Save(IRepository repository, IArchiver archiver, List<IBackupObject> backupObjects, string fullPathOfRestorePoint);
}