using System;
using System.IO.Compression;
using Backups.Algorithms;
using Backups.Archivers;
using Backups.Interfaces;
using Backups.Repository;
using Zio;
using Zio.FileSystems;

namespace Backups.Entities;

public class BackupTask : IBackupTask
{
    private readonly List<IBackupObject> _backupObjects = new ();
    private int _idRestorePoints = 1;

    public BackupTask(string name, IRepository repository, IAlgorithm algorithm, IArchiver archiver)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new Exception();
        }

        Name = name;
        Repository = repository;
        Algorithm = algorithm;
        Archiver = archiver;
        Backup = new Backup();
        Repository.CreateDirectory($"{repository.FullPath}/{name}");
    }

    public string Name { get; }
    public IRepository Repository { get; }
    public IAlgorithm Algorithm { get; }
    public IBackup Backup { get; }
    public IArchiver Archiver { get; }
    public IReadOnlyCollection<IBackupObject> BackupObjects() => _backupObjects;

    public void AddBackupObject(IBackupObject backupObject)
    {
        if (_backupObjects.Contains(backupObject))
        {
            throw new Exception();
        }

        _backupObjects.Add(backupObject);
    }

    public void RemoveBackupObject(IBackupObject backupObject)
    {
        if (!_backupObjects.Remove(backupObject))
        {
            throw new Exception();
        }
    }

    public RestorePoint Working()
    {
        string restorePointPath = $"{Name}/{_idRestorePoints}/";
        Repository.CreateDirectory(restorePointPath);

        List<Storage> storages =
            Algorithm.Save(Repository, Archiver, _backupObjects, $"{Repository.FullPath}/{Name}/{_idRestorePoints}");

        var restorePoint = new RestorePoint(restorePointPath, Repository, _backupObjects);
        Backup.AddRestorePoint(restorePoint);
        _idRestorePoints++;
        return restorePoint;
    }
}