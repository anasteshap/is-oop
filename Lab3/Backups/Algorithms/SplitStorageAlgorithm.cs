using Backups.Archivers;
using Backups.Component;
using Backups.Entities;
using Backups.Inter;
using Backups.Repository;

namespace Backups.Algorithms;

public class SplitStorageAlgorithm : IAlgorithm
{
    public List<Storage> Save(IRepository repository, IArchiver archiver, List<IBackupObject> backupObjects, string pathOfRestorePoint)
    {
        backupObjects
            .ForEach(x => archiver.Archive(
                    new List<IComponent>() { repository.GetRepositoryComponent(x.FullName) }, // тут передается один компонент всегда
                    repository,
                    CreatePathForZip(pathOfRestorePoint, x.FullName + "_")));

        var storages = new List<Storage>();
        return storages;
    }

    private string CreatePathForZip(string folderPath, string zipName) => Path.Combine(folderPath, $"{zipName}.zip");
}