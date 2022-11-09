using System.IO.Compression;
using Backups.Archivers;
using Backups.Component;
using Backups.Entities;
using Backups.Interfaces;
using Backups.Repository;

namespace Backups.Algorithms;

public class SingleStorageAlgorithm : IAlgorithm
{
    public List<Storage> Save(IRepository repository, IArchiver archiver, List<IBackupObject> backupObjects, string fullPathOfRestorePoint)
    {
        var repoComponents = backupObjects
            .Select(x => x.GetRepoComponent())
            .ToList();

        archiver.Archive(repoComponents, repository, CreatePathForZip(fullPathOfRestorePoint));

        var storages = new List<Storage>();
        return storages;
    }

    private string CreatePathForZip(string folderPath) => Path.Combine(folderPath, "Archive.zip");
}