using System.IO.Compression;
using Backups.Archivers;
using Backups.Component;
using Backups.Entities;
using Backups.Inter;
using Backups.Repository;

namespace Backups.Algorithms;

public class SingleStorageAlgorithm : IAlgorithm
{
    public List<Storage> Save(IRepository repository, IArchiver archiver, List<IBackupObject> backupObjects, string pathOfRestorePoint)
    {
        var repoComponents = backupObjects
            .Select(x => repository.GetRepositoryComponent(x.FullName))
            .ToList();

        archiver.Archive(repoComponents, repository, CreatePathForZip(pathOfRestorePoint));

        var storages = new List<Storage>();
        return storages;
    }

    private string CreatePathForZip(string folderPath) => Path.Combine(folderPath, "Archive.zip");
}