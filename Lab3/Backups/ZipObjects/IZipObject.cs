using System.IO.Compression;
using Backups.Component;

namespace Backups.ZipObjects;

public interface IZipObject
{
    string Name { get; }
    IComponent GetRepoComponent(ZipArchiveEntry entry);
}