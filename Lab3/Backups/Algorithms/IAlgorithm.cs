using Backups.Entities;
using Backups.Inter;
using Backups.Repository;

namespace Backups.Algorithms;

public interface IAlgorithm
{
    List<Storage> Save(IRepository repository, List<IBackupObject> backupObjects, string pathOfRestorePoint);
}