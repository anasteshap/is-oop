using Backups.Algorithms;
using Backups.Archivers;
using Backups.Entities;
using Backups.Repository;
using Xunit;

namespace Backups.Test;

public class BackupTests
{
    [Fact(Skip = "fs doesn't work in gh")]
    public void Test1()
    {
        string systemPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var rep = new FileSystemRepository(systemPath + "/rep");
        var backupObject1 = new BackupObject(rep, "copy.xlsx");
        var backupObject2 = new BackupObject(rep, "temp");

        var backupTask1 = new BackupTask("Task1", rep, new SplitStorageAlgorithm(), new Archiver());
        backupTask1.AddBackupObject(backupObject1);
        backupTask1.AddBackupObject(backupObject2);
        RestorePoint rp11 = backupTask1.Working();
        bool bool1 = rep.Exists(Path.Combine(rp11.RelativePath, "temp.zip"));
        bool bool2 = rep.Exists(Path.Combine(rp11.RelativePath, "copy.xlsx.zip"));
        Assert.True(bool1);
        Assert.True(bool2);

        backupTask1.RemoveBackupObject(backupObject1);
        RestorePoint rp12 = backupTask1.Working();
        bool bool3 = rep.Exists(Path.Combine(rp12.RelativePath, "copy.xlsx.zip"));
        Assert.False(bool3);

        var backupTask2 = new BackupTask("Task2", rep, new SingleStorageAlgorithm(), new Archiver());
        backupTask2.AddBackupObject(backupObject1);
        backupTask2.AddBackupObject(backupObject2);
        backupTask2.Working();
    }

    [Fact]
    public void Test2()
    {
        var rep = new InMemoryRepository("/");
        rep.CreateDirectory("a");
        rep.CreateFile("a/copy.txt");
        rep.CreateFile("copy.txt");
        rep.CreateDirectory("b");

        var backupObject1 = new BackupObject(rep, "a");
        var backupObject2 = new BackupObject(rep, "b");
        var backupObject3 = new BackupObject(rep, "copy.txt");

        var backupTask1 = new BackupTask("Task1", rep, new SplitStorageAlgorithm(), new Archiver());
        backupTask1.AddBackupObject(backupObject1);
        backupTask1.AddBackupObject(backupObject2);
        RestorePoint rp11 = backupTask1.Working();
        Assert.True(rep.DirectoryExists($"{rp11.RelativePath}"));

        backupTask1.RemoveBackupObject(backupObject1);
        RestorePoint rp12 = backupTask1.Working();
        Assert.True(rep.DirectoryExists($"{rp12.RelativePath}"));

        Assert.True(backupTask1.RestorePoints().Count == 2);

        var backupTask2 = new BackupTask("Task2", rep, new SingleStorageAlgorithm(), new Archiver());
        backupTask2.AddBackupObject(backupObject1);
        backupTask2.Working();

        Assert.True(rep.DirectoryExists("a"));
        Assert.True(rep.DirectoryExists("b"));
        Assert.True(rep.DirectoryExists("Task1"));
        Assert.True(rep.DirectoryExists("Task2"));
    }
}