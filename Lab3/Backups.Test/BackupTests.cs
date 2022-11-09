using System.IO.Compression;
using Backups.Algorithms;
using Backups.Archivers;
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
        string systemPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var rep = new FileSystemRepository(systemPath + "/rep");
        var backupObject1 = new BackupObject(rep, "copy.xlsx");
        var backupObject2 = new BackupObject(rep, "temp");

        var backupTask1 = new BackupTask("Task1", rep, new SplitStorageAlgorithm(), new Archiver());
        backupTask1.AddBackupObject(backupObject1);
        backupTask1.AddBackupObject(backupObject2);
        backupTask1.Working();
        backupTask1.RemoveBackupObject(backupObject1);
        backupTask1.Working();

        var backupTask2 = new BackupTask("Task2", rep, new SingleStorageAlgorithm(), new Archiver());
        backupTask2.AddBackupObject(backupObject1);
        backupTask2.AddBackupObject(backupObject2);
        backupTask2.Working();
    }

    [Fact]
    public void Test2()
    {
        var rep = new InMemoryRepository("/");
        rep.CreateDirectory("temp");
        var backupObject1 = new BackupObject(rep, "temp");

        var backupTask1 = new BackupTask("Task1", rep, new SplitStorageAlgorithm(), new Archiver());
        backupTask1.AddBackupObject(backupObject1);
        backupTask1.Working();

        var backupTask2 = new BackupTask("Task2", rep, new SingleStorageAlgorithm(), new Archiver());
        backupTask2.AddBackupObject(backupObject1);
        backupTask2.Working();
    }
}