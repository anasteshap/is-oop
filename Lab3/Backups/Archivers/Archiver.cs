using System.IO.Compression;
using Backups.Component;
using Backups.Repository;
using Backups.Storage;
using Backups.Visitor;
using Backups.ZipObjects;

namespace Backups.Archivers;

public class Archiver : IArchiver
{
    public ZipStorage Archive(IComponent component, IRepository repository, string zipPath)
    {
        return Archive(new List<IComponent>() { component }, repository, zipPath);
    }

    public ZipStorage Archive(List<IComponent> components, IRepository repository, string zipPath) // /../zipPath = Archive.zip or /../temp.zip
    {
        using Stream stream = repository.OpenStream(zipPath);
        using var archive = new ZipArchive(stream, ZipArchiveMode.Update);
        var visitor = new ArchiveVisitor(archive);

        components.ForEach(x => x.Accept(visitor));
        string zipName = Path.GetFileName(zipPath);
        var zipFolder = new ZipFolder(zipName, visitor.GetZipObjects().ToList()); // zipName = Archive.zip
        return new ZipStorage(zipPath, repository, zipFolder); // zipPath = /.../Archive.zip
    }
}