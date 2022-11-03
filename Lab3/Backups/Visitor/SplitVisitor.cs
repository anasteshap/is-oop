using System.IO.Compression;
using Backups.Component;
using Backups.Repository;

namespace Backups.Visitor;

public class SplitVisitor : IVisitor
{
    private readonly Stack<ZipArchive> _archives = new ();
    private readonly string _pathOfRestorePoint;

    public SplitVisitor(string pathOfRestorePoint)
    {
        if (string.IsNullOrEmpty(pathOfRestorePoint))
        {
            throw new Exception();
        }

        _pathOfRestorePoint = pathOfRestorePoint;
    }

    public void CreateZipFile(FileComponent fileComponent)
    {
        string fileFullName = Path.Combine(fileComponent.Repository.FullName, fileComponent.FullName);
        string zipPath = Path.Combine(_pathOfRestorePoint, $"{Path.GetFileName(fileFullName)}.zip");
        using Stream streamFrom = fileComponent.Repository.OpenStream(fileComponent.FullName);

        if (_archives.Count == 0)
        {
            using Stream zipToOpen = fileComponent.Repository.OpenStream(zipPath);
            using (var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
            {
                Stream stream = archive.CreateEntry(Path.GetFileName(fileFullName), CompressionLevel.Optimal).Open();
                streamFrom.CopyTo(stream);
            }
        }
        else
        {
            var oldArchive = _archives.Peek();
            Stream oldStream = oldArchive.CreateEntry(Path.GetFileName(fileFullName), CompressionLevel.Optimal).Open();
            streamFrom.CopyTo(oldStream);
        }
    }

    public void CreateZipFile(FolderComponent folderComponent)
    {
        string folderFullName = Path.Combine(folderComponent.Repository.FullName, folderComponent.FullName);
        string zipPath = Path.Combine(_pathOfRestorePoint, $"{Path.GetFileName(folderFullName)}.zip");

        Stream zipToOpen;
        if (_archives.Count == 0)
        {
            zipToOpen = folderComponent.Repository.OpenStream(zipPath);
        }
        else
        {
            var entry = _archives.Peek().CreateEntry(Path.GetFileName(folderFullName), CompressionLevel.Optimal);
            zipToOpen = entry.Open();
        }

        var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update);
        _archives.Push(archive);

        /*foreach (var comp in folderComponent.Components)
        {
            this.CreateZipFile(comp);
        }*/

        folderComponent.Accept(this);
        _archives.Pop();
    }
}