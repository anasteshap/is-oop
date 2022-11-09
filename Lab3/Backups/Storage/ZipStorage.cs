using Backups.Component;
using Backups.Repository;
using Backups.ZipObjects;

namespace Backups.Storage;

public class ZipStorage : IStorage
{
    public ZipStorage(string relativePath, IRepository repository, ZipFolder zipFolder)
    {
        if (string.IsNullOrEmpty(relativePath))
        {
            throw new Exception();
        }

        RelativePath = relativePath;
        Repository = repository;
        ZipFolder = zipFolder;
    }

    public string RelativePath { get; }
    public IRepository Repository { get; }

    public ZipFolder ZipFolder { get; }

    public IComponent GetRepoComponents()
    {
        throw new NotImplementedException();
    }
}