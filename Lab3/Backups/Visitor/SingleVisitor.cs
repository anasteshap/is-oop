using System.IO.Compression;
using Backups.Component;
using Backups.Repository;

namespace Backups.Visitor;

public class SingleVisitor : IVisitor
{
    private readonly Stack<ZipArchive> _archives = new ();

    public SingleVisitor(string zipPath, IRepository repository) // основной репозиторий
    {
        Stream zipToOpen = repository.OpenStream(zipPath);
        using (var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
        {
            _archives.Push(archive);
        }
    }

    public void CreateZipFile(FileComponent fileComponent)
    {
        /*var entry = _archives.Peek().GetEntry();
        using (var zipToOpen = new FileStream(zipName, FileMode.OpenOrCreate))
        {
            using (var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
            {
                if (File.Exists(obj.FullName))
                {
                    archive.CreateEntryFromFile(obj.FullName, Path.GetFileName(obj.FullName));
                }
            }
        }*/
    }

    public void CreateZipFile(FolderComponent folderComponent)
    {
        throw new NotImplementedException();
    }
}