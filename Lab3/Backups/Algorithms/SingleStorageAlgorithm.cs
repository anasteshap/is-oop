using System.IO.Compression;
using Backups.Entities;
using Backups.Inter;
using Backups.Repository;

namespace Backups.Algorithms;

public class SingleStorageAlgorithm : IAlgorithm
{
    public List<Storage> Save(IRepository repository, List<IBackupObject> backupObjects, string pathOfRestorePoint)
    {
        throw new NotImplementedException();
    }
}