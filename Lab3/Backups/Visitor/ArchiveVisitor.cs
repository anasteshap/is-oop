using System.IO.Compression;
using Backups.Component;

namespace Backups.Visitor;

public class ArchiveVisitor : IVisitor
{
    private readonly Stack<ZipArchive> _archives = new ();

    public ArchiveVisitor(ZipArchive zipArchive)
    {
        if (zipArchive is null)
        {
            throw new Exception();
        }

        _archives.Push(zipArchive);
    }

    public void CreateZipFile(FileComponent fileComponent)
    {
        string fileFullName = Path.Combine(fileComponent.Repository.FullName, fileComponent.FullName);
        using Stream streamFrom = fileComponent.Repository.OpenStream(fileComponent.FullName);

        ZipArchive archive = _archives.Peek();
        Stream zipToOpen = archive.CreateEntry(Path.GetFileName(fileFullName), CompressionLevel.Optimal).Open();
        streamFrom.CopyTo(zipToOpen);
    }

    public void CreateZipFile(FolderComponent folderComponent)
    {
        string folderFullName = Path.Combine(folderComponent.Repository.FullName, folderComponent.FullName);
        Stream zipToOpen = _archives.Peek()
            .CreateEntry($"{Path.GetFileName(folderFullName)}.zip", CompressionLevel.Optimal)
            .Open();

        using var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update);
        _archives.Push(archive);

        foreach (IComponent subComponent in folderComponent.Components)
        {
            subComponent.Accept(this);
        }

        _archives.Pop();
    }
}