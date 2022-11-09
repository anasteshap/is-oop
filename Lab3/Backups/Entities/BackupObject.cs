using Backups.Component;
using Backups.Interfaces;
using Backups.Repository;

namespace Backups.Entities;

public class BackupObject : IBackupObject
{
    public BackupObject(IRepository repository, string fileName)
    {
        if (string.IsNullOrEmpty(fileName) || repository is null)
        {
            throw new Exception();
        }

        RelativePath = fileName;
        if (!repository.Exists(RelativePath))
        {
            throw new Exception();
        }

        Repository = repository;
    }

    public string RelativePath { get; }
    public IRepository Repository { get; }
    public IComponent GetRepoComponent() => Repository.GetRepositoryComponent(RelativePath);
}