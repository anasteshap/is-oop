using Backups.Archivers;
using Backups.Entities;
using Backups.Inter;
using Backups.Repository;

namespace Backups.Algorithms;

public interface IAlgorithm
{
    List<Storage> Save(IRepository repository, IArchiver archiver, List<IBackupObject> backupObjects, string pathOfRestorePoint);
}