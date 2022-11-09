using Backups.Archivers;
using Backups.Interfaces;
using Backups.Repository;
using Backups.Storage;

namespace Backups.Algorithms;

public class SingleStorageAlgorithm : IAlgorithm
{
    public IStorage Save(IRepository repository, IArchiver archiver, List<IBackupObject> backupObjects, string fullPathOfRestorePoint)
    {
        string zipPath = CreatePathForZip(fullPathOfRestorePoint);
        var repoComponents = backupObjects
            .Select(x => x.GetRepoComponent())
            .ToList();

        return archiver.Archive(repoComponents, repository, zipPath);
    }

    private string CreatePathForZip(string folderPath) => Path.Combine(folderPath, "Archive.zip");
}