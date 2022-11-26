using Backups.Algorithms;
using Backups.Archivers;
using Backups.Exceptions;
using Backups.Interfaces;
using Backups.Repository;
using Backups.Storage;

namespace Backups.Entities;

public class BackupTask : IBackupTask
{
    private readonly List<IBackupObject> _backupObjects = new ();
    private readonly IBackup _backup = new Backup();
    public BackupTask(string name, IRepository repository, IAlgorithm algorithm, IArchiver archiver)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw NullException.InvalidName();
        }

        Name = name;
        Repository = repository;
        Algorithm = algorithm;
        Archiver = archiver;
        Repository.CreateDirectory($"{repository.FullPath}/{name}");
    }

    public string Name { get; }
    public IRepository Repository { get; }
    public IAlgorithm Algorithm { get; }
    public IArchiver Archiver { get; }
    public IReadOnlyCollection<IBackupObject> BackupObjects() => _backupObjects;
    public IReadOnlyCollection<RestorePoint> RestorePoints() => _backup.RestorePoints;

    public void AddBackupObject(IBackupObject backupObject)
    {
        if (_backupObjects.Contains(backupObject))
        {
            throw BackupException.BackupObjectAlreadyExists(backupObject.RelativePath);
        }

        _backupObjects.Add(backupObject);
    }

    public void RemoveBackupObject(IBackupObject backupObject)
    {
        if (!_backupObjects.Remove(backupObject))
        {
            throw BackupException.BackupObjectDoesNotExist(backupObject.RelativePath);
        }
    }

    public RestorePoint Working()
    {
        DateTime dateTime = DateTime.Now;
        string data = $"{dateTime:dd.MM.yyyy}";
        string restorePointPath = $"{Name}/{data} {dateTime.Hour}h.{dateTime.Minute}m.{dateTime.Second}s.{dateTime.Millisecond}ms/";
        Repository.CreateDirectory(restorePointPath);

        IStorage storage =
            Algorithm.Save(Repository, Archiver, _backupObjects, $"{Repository.FullPath}/{restorePointPath}");

        var restorePoint = new RestorePoint(restorePointPath, dateTime, storage, _backupObjects);
        _backup.AddRestorePoint(restorePoint);
        return restorePoint;
    }
}