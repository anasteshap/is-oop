using System.IO.Compression;
using Backups.Component;
using Backups.Repository;
using Backups.Visitor;

namespace Backups.Archivers;

public class Archiver : IArchiver
{
    public void Archive(List<IComponent> components, IRepository repository, string zipPath)
    {
        Stream stream = repository.OpenStream(zipPath);
        using var archive = new ZipArchive(stream, ZipArchiveMode.Update);
        var visitor = new ArchiveVisitor(archive);

        components.ForEach(x => x.Accept(visitor));
    }
}