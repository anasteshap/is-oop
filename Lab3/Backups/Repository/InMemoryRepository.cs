using Backups.Component;
using Backups.Exceptions;
using Zio;
using Zio.FileSystems;

namespace Backups.Repository;

public class InMemoryRepository : IRepository, IDisposable
{
    private readonly Func<string, Stream> _streamCreator;
    private readonly Func<string, IReadOnlyCollection<IComponent>> _componentsCreator;
    private readonly MemoryFileSystem _fs;
    private bool _disposed;

    public InMemoryRepository(string fullPath)
    {
        if (string.IsNullOrEmpty(fullPath))
        {
            throw NullException.InvalidName();
        }

        _fs = new MemoryFileSystem();
        FullPath = fullPath;
        _streamCreator = OpenStream;
        _componentsCreator = GetComponents;
    }

    ~InMemoryRepository() => Dispose(false);

    public string FullPath { get; }

    public Stream OpenStream(string path)
    {
        path = Path.GetFullPath(path, FullPath);
        return _fs.OpenFile(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
    }

    public void SaveTo(string path, Stream stream)
    {
        path = Path.GetFullPath(path, FullPath);
        using Stream newStream = _fs.OpenFile(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        stream.CopyTo(newStream);
    }

    public IRepository GetSubRepository(string partialPath) => new InMemoryRepository(Path.Combine(FullPath, partialPath));

    public IComponent GetRepositoryComponent(string partialPath)
    {
        if (_fs.DirectoryExists(Path.GetFullPath(partialPath, FullPath)))
        {
            return new FolderComponent(Path.GetFileName(partialPath), () => _componentsCreator(Path.GetFullPath(partialPath, FullPath)));
        }

        return new FileComponent(Path.GetFileName(partialPath), () => _streamCreator(Path.GetFullPath(partialPath, FullPath)));
    }

    public IReadOnlyCollection<IComponent> GetComponents(string path)
    {
        DirectoryEntry dir = _fs.GetDirectoryEntry(Path.GetFullPath(path, FullPath));
        var directories = dir
            .EnumerateDirectories()
            .Where(x => DirectoryExists(x.FullName))
            .Select(x => new FolderComponent(x.Name, () => _componentsCreator(x.FullName)))
            .ToList();
        var files = dir
            .EnumerateFiles()
            .Where(x => FileExists(x.FullName))
            .Select(x => new FileComponent(x.Name, () => _streamCreator(x.FullName)))
            .ToList();
        return new List<IComponent>().Concat(directories).Concat(files).ToList();
    }

    public void CreateDirectory(string path) => _fs.CreateDirectory(Path.GetFullPath(path, FullPath));
    public void CreateFile(string path) => _fs.CreateFile(Path.GetFullPath(path, FullPath)).Dispose();

    public bool Exists(string path)
    {
        path = Path.GetFullPath(path, FullPath);
        return _fs.FileExists(path) || _fs.DirectoryExists(path);
    }

    public bool FileExists(string path)
    {
        path = Path.GetFullPath(path, FullPath);
        return _fs.FileExists(path);
    }

    public bool DirectoryExists(string path)
    {
        path = Path.GetFullPath(path, FullPath);
        return _fs.DirectoryExists(path);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _fs.Dispose();
            }

            _disposed = true;
        }
    }
}