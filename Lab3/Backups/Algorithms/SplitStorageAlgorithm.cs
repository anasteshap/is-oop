using Backups.Archivers;
using Backups.Component;
using Backups.Entities;
using Backups.Interfaces;
using Backups.Repository;

namespace Backups.Algorithms;

public class SplitStorageAlgorithm : IAlgorithm
{
    public List<Storage> Save(IRepository repository, IArchiver archiver, List<IBackupObject> backupObjects, string fullPathOfRestorePoint)
    {
        backupObjects
            .ForEach(x => archiver.Archive(
                    new List<IComponent>() { x.GetRepoComponent() }, // тут передается один компонент всегда
                    repository,
                    CreatePathForZip(fullPathOfRestorePoint, x.RelativePath + "_")));

        var storages = new List<Storage>();
        return storages;
    }

    private string CreatePathForZip(string folderPath, string zipName) => Path.Combine(folderPath, $"{zipName}.zip");
}