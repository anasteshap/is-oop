using System.IO.Compression;
using Backups.Component;
using Backups.Exceptions;
using Backups.Repository;
using Backups.ZipObjects;

namespace Backups.Storage;

public class ZipStorage : IStorage
{
    public ZipStorage(string relativePath, IRepository repository, ZipFolder zipFolder) // тут ентри передавать?
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

    public int GetZipArchivesCount() => 1;

    public IReadOnlyCollection<IComponent> GetRepoComponents()
    {
        var components = new List<IComponent>();
        using Stream stream = Repository.OpenStream(RelativePath);

        /*if (Equals(Path.GetFileName(RelativePath), "Archive.zip"))
        {
            using var archive = new ZipArchive(stream, ZipArchiveMode.Read);
            var entries = archive.Entries.ToList();
            entries.ForEach(x => components.Add(x.GetRepoComponent());

            var zipFiles = ZipFolder.ZipFiles().ToList();
            zipFiles.ForEach(x => components.Add(x.GetRepoComponent()));
        }
        zipFiles.ForEach(x => components.Add(x.GetRepoComponent())); // сюда ентри передаем*/
        return components;
    }
}