using System.IO.Compression;
using Backups.Component;

namespace Backups.ZipObjects;

public class ZipFile : IZipObject
{
    public ZipFile(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new Exception();
        }

        Name = name;
    }

    public string Name { get; }

    public IComponent GetRepoComponent(ZipArchiveEntry entry)
    {
        throw new NotImplementedException();
    }
}