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

        RelativePath = fileName;
        if (!repository.Exists(RelativePath))
        {
            throw RepositoryException.ObjectDoesNotExist(RelativePath);
        }

        Repository = repository;
    }

    public string RelativePath { get; }
    public IRepository Repository { get; }
    public IComponent GetRepoComponent() => Repository.GetRepositoryComponent(RelativePath);
}