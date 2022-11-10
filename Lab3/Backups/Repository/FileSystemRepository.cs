using System.Collections.ObjectModel;
using Backups.Component;
using Backups.Exceptions;

namespace Backups.Repository;

public class FileSystemRepository : IRepository
{
    private readonly Func<string, Stream> _streamCreator;
    private readonly Func<string, IReadOnlyCollection<IComponent>> _componentsCreator;

    public FileSystemRepository(string fullPath)
    {
        if (string.IsNullOrEmpty(fullPath))
        {
            throw NullException.InvalidName();
        }

        FullPath = fullPath;
        _streamCreator = OpenStream;
        _componentsCreator = GetComponents;
    }

    public string FullPath { get; }

    public Stream OpenStream(string path) =>
        new FileStream(Path.GetFullPath(path, FullPath), FileMode.OpenOrCreate, FileAccess.ReadWrite);

    public void SaveTo(string path, Stream stream)
    {
        path = Path.GetFullPath(path, FullPath);
        var newStream = new FileStream(Path.GetFullPath(path, FullPath), FileMode.OpenOrCreate, FileAccess.Write);
        stream.CopyTo(newStream);
    }

    public IRepository GetSubRepository(string partialPath) => new FileSystemRepository(Path.Combine(FullPath, partialPath));

    public IComponent GetRepositoryComponent(string partialPath)
    {
        if (DirectoryExists(partialPath))
        {
            return new FolderComponent(Path.GetFileName(partialPath), () => _componentsCreator(partialPath));
        }

        return new FileComponent(Path.GetFileName(partialPath), () => _streamCreator(partialPath));
    }

    public IReadOnlyCollection<IComponent> GetComponents(string path)
    {
        path = Path.GetFullPath(path, FullPath);
        if (!Directory.Exists(path))
        {
            throw RepositoryException.DirectoryDoesNotExist(path);
        }

        var directories = Directory.GetDirectories(path)
            .Select(x => new FolderComponent(Path.GetFileName(x), () => _componentsCreator(x))) // x -> Path.GetRelativePath(FullPath, a)
            .ToList();
        var files = Directory.GetFiles(path)
            .Select(x => new FileComponent(Path.GetFileName(x), () => _streamCreator(x)))
            .ToList();

        return new List<IComponent>().Concat(directories).Concat(files).ToList();
    }

    public void CreateDirectory(string path) => Directory.CreateDirectory(Path.GetFullPath(path, FullPath));

    public bool Exists(string path)
    {
        path = Path.GetFullPath(path, FullPath);
        return Directory.Exists(path) || File.Exists(path);
    }

    public bool FileExists(string path)
    {
        path = Path.GetFullPath(path, FullPath);
        return File.Exists(path);
    }

    public bool DirectoryExists(string path)
    {
        path = Path.GetFullPath(path, FullPath);
        return Directory.Exists(path);
    }
}