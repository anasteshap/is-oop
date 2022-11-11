using Backups.Component;
using Backups.Exceptions;
using Backups.Interfaces;
using Backups.Repository;

namespace Backups.Entities;

public class BackupObject : IBackupObject
{
    public BackupObject(IRepository repository, string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw NullException.InvalidName();
        }

        if (!repository.Exists(fileName))
        {
            throw RepositoryException.ObjectDoesNotExist(Path.Combine(repository.FullPath, fileName));
        }

        Repository = repository;
        RelativePath = fileName;
    }

    public string RelativePath { get; }
    public IRepository Repository { get; }
    public IComponent GetRepoComponent() => Repository.GetRepositoryComponent(RelativePath);
}