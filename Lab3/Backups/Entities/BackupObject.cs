using Backups.Inter;
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

        FullName = fileName;
        if (!repository.Exists(FullName))
        {
            throw new Exception();
        }

        Repository = repository;
    }

    public string FullName { get; }
    public IRepository Repository { get; }
}