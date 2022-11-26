using System.IO.Compression;
using Backups.Component;
using Backups.Exceptions;
using Backups.Repository;
using Backups.ZipObjects;

namespace Backups.Storage;

public class ZipStorage : IStorage
{
    public ZipStorage(string relativePath, IRepository repository, ZipFolder zipFolder)
    {
        if (string.IsNullOrEmpty(relativePath))
        {
            throw NullException.InvalidName();
        }

        RelativePath = relativePath;
        Repository = repository;
        ZipFolder = zipFolder;
    }

    public string RelativePath { get; }
    public IRepository Repository { get; }
    public ZipFolder ZipFolder { get; }

    public IReadOnlyCollection<IComponent> GetRepoComponents()
    {
        using Stream stream = Repository.OpenStream(RelativePath);
        var archive = new ZipArchive(stream, ZipArchiveMode.Read);

        var zipFiles = ZipFolder.ZipFiles().ToList();
        return archive.Entries
            .Select(e => zipFiles.Single(x => x.Name.Equals(e.Name)).GetRepoComponent(e))
            .ToList();
    }
}