using Backups.Component;
using Backups.Entities;
using Backups.Inter;
using Backups.Repository;
using Backups.Visitor;

namespace Backups.Algorithms;

public class SplitStorageAlgorithm : IAlgorithm
{
    public List<Storage> Save(IRepository repository, List<IBackupObject> backupObjects, string pathOfRestorePoint)
    {
        var storages = new List<Storage>();
        var visitor = new SplitVisitor(pathOfRestorePoint);

        foreach (IBackupObject obj in backupObjects)
        {
            if (obj.Repository.DirectoryExists(obj.FullName))
            {
                var folder = new FolderComponent(repository, obj.FullName);
                folder.Accept(visitor);
            }
            else if (obj.Repository.FileExists(obj.FullName))
            {
                var file = new FileComponent(repository, obj.FullName);
                file.Accept(visitor);
            }
        }

        return storages;
    }
}