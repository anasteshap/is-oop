using Backups.Component;
using Zio;
using Zio.FileSystems;

namespace Backups.Repository;

public class InMemoryRepository : IRepository, IDisposable
{
    private readonly Func<string, Stream> _streamCreator;
    private readonly Func<string, IReadOnlyCollection<IComponent>> _componentsCreator;
    private readonly MemoryFileSystem _fs;
    private bool _disposed;

    public InMemoryRepository()
    {
        _fs = new MemoryFileSystem();
        FullPath = "/";
        _streamCreator = OpenStream;
        _componentsCreator = GetComponents;
    }

    ~InMemoryRepository() => Dispose(false);

    public string FullPath { get; }

    public Stream OpenStream(string path)
    {
        path = Path.GetFullPath(path);
        return _fs.OpenFile(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
    }

    public void SaveTo(string path, Stream stream)
    {
        throw new NotImplementedException();
    }

    public IRepository GetSubRepository(string partialPath)
    {
        throw new NotImplementedException();

        /*var a = new SubFileSystem(_fs, partialPath);
        a.
        var newFs = new InMemoryRepository();
        newFs.RelativePath = RelativePath*/
    }

    public IComponent GetRepositoryComponent(string partialPath)
    {
        if (_fs.DirectoryExists(Path.GetFullPath(partialPath, FullPath)))
        {
            return new FolderComponent(Path.GetFileName(partialPath), () => _componentsCreator(partialPath));
        }

        return new FileComponent(Path.GetFileName(partialPath), () => _streamCreator(partialPath));
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
            .Select(x => new FolderComponent(x.Name, () => _componentsCreator(x.FullName)))
            .ToList();
        return new List<IComponent>().Concat(directories).Concat(files).ToList();
    }

    public IReadOnlyCollection<string> GetRelativePathsOfFolderSubFiles(string path)
    {
        throw new Exception();
    }

    public void CreateDirectory(string path) => _fs.CreateDirectory(Path.GetFullPath(path, FullPath));

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