using System.IO.Compression;
using Backups.Algorithms;
using Backups.Component;
using Backups.Entities;
using Backups.Repository;
using Xunit;
namespace Backups.Test;

public class BackupTests
{
    [Fact]
    public void Test1()
    {
        var systemPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var rep = new FileSystemRepository(systemPath + "/rep");

        var backupTask1 = new BackupTask("Task1", rep, new SplitStorageAlgorithm());
        var backupObject1 = new BackupObject(rep, "copy.xlsx");
        var backupObject2 = new BackupObject(rep, "temp");
        backupTask1.AddBackupObject(backupObject1);
        backupTask1.AddBackupObject(backupObject2);

        backupTask1.Working();

        /*var list = rep.GetRelativePathsOfFolderSubFiles(systemPath + "/rep/").ToList();
        Assert.Contains("copy.xlsx", list);

        var pathOfRestorePoint = $"{rep.FullName}/Task1/1";
        var fileComponent = new FileComponent(rep, "temp");
        string fileFullName = $"{fileComponent.Repository.FullName}/{fileComponent.FullName}";
        string zipPath = $"{pathOfRestorePoint}/{Path.GetFileName(fileFullName)}.zip";

        using (Stream zipToOpen = fileComponent.Repository.OpenStream(zipPath))
        {
            using (var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
            {
            }
        }*/
    }
}