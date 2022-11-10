using Backups.Component;
using Backups.Exceptions;
using Backups.ZipObjects;

namespace Backups.Storage;

public class SplitStorage : IStorage
{
    private readonly List<ZipStorage> _storages = new ();

    public IReadOnlyCollection<ZipStorage> Storages => _storages;
    public IReadOnlyCollection<IComponent> GetRepoComponents()
    {
        throw new NotImplementedException();
    }

    public void AddZipStorage(ZipStorage zipStorage)
    {
        if (_storages.Contains(zipStorage))
        {
            throw StorageException.ZipStorageAlreadyExists(zipStorage.RelativePath);
        }

        _storages.Add(zipStorage);
    }
}