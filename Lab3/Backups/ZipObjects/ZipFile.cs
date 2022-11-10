using System.IO.Compression;
using Backups.Component;
using Backups.Exceptions;

namespace Backups.ZipObjects;

public class ZipFile : IZipObject
{
    public ZipFile(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw NullException.InvalidName();
        }

        Name = name;
    }

    public string Name { get; }

    public IComponent GetRepoComponent(ZipArchiveEntry entry)
    {
        throw new NotImplementedException();
    }
}