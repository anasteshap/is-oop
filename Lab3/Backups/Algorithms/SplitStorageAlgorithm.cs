using Backups.Archivers;
using Backups.Component;
using Backups.Interfaces;
using Backups.Repository;
using Backups.Storage;

namespace Backups.Algorithms;

public class SplitStorageAlgorithm : IAlgorithm
{
    public SplitStorage Save(IRepository repository, IArchiver archiver, List<IBackupObject> backupObjects, string fullPathOfRestorePoint)
    {
        var storage = new SplitStorage();

        foreach (IBackupObject obj in backupObjects)
        {
            string zipPath = CreatePathForZip(fullPathOfRestorePoint, obj.RelativePath + "_");
            ZipStorage zipStorage = archiver.Archive(obj.GetRepoComponent(), repository, zipPath);
            storage.AddZipStorage(zipStorage);
        }

        return storage;
    }

    private string CreatePathForZip(string folderPath, string zipName) => Path.Combine(folderPath, $"{zipName}.zip");
}