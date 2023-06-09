using System.IO.Compression;
using Backups.Component;
using Backups.Exceptions;
using Backups.ZipObjects;
using ZipFile = Backups.ZipObjects.ZipFile;

namespace Backups.Visitor;

public class ArchiveFileComponentVisitor : IFileComponentVisitor
{
    private readonly Stack<ZipArchive> _archives = new ();
    private readonly Stack<List<IZipObject>> _listsOfZipFiles = new ();

    public ArchiveFileComponentVisitor(ZipArchive zipArchive)
    {
        if (zipArchive is null)
        {
            throw NullException.InvalidName();
        }

        _archives.Push(zipArchive);
        _listsOfZipFiles.Push(new List<IZipObject>());
    }

    public IReadOnlyCollection<IZipObject> GetZipObjects()
    {
        if (_listsOfZipFiles.Count is 0 or > 1)
        {
            throw VisitorException.InvalidZipObjects();
        }

        return _listsOfZipFiles.Peek();
    }

    public void Visit(FileComponent fileComponent)
    {
        using Stream streamFrom = fileComponent.OpenStream();
        ZipArchive archive = _archives.Peek();
        using Stream zipToOpen = archive.CreateEntry(fileComponent.Name, CompressionLevel.Optimal).Open();
        streamFrom.CopyTo(zipToOpen);

        var zipFile = new ZipFile(fileComponent.Name); // без .zip
        _listsOfZipFiles.Peek().Add(zipFile);
    }

    public void Visit(FolderComponent folderComponent)
    {
        using Stream zipToOpen = _archives.Peek().CreateEntry($"{folderComponent.Name}.zip", CompressionLevel.Optimal).Open();
        using var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update);
        _archives.Push(archive);
        _listsOfZipFiles.Push(new List<IZipObject>());

        foreach (IComponent subComponent in folderComponent.Components)
        {
            subComponent.Accept(this);
        }

        List<IZipObject> listOfZipFiles = _listsOfZipFiles.Pop();
        var zipFolder = new ZipFolder($"{folderComponent.Name}.zip", listOfZipFiles); // c .zip
        _listsOfZipFiles.Peek().Add(zipFolder);

        _archives.Pop();
    }
}