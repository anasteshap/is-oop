using System.IO.Compression;
using Backups.Component;

namespace Backups.ZipObjects;

public class ZipFolder : IZipObject
{
    private readonly List<IZipObject> _zipFiles;
    public ZipFolder(string name, List<IZipObject> zipFiles)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new Exception();
        }

        Name = name;
        _zipFiles = zipFiles;
    }

    public string Name { get; }

    public IReadOnlyCollection<IZipObject> ZipFiles() => _zipFiles;

    public IComponent GetRepoComponent(ZipArchiveEntry entry)
    {
        throw new NotImplementedException();
    }
}