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

    public InMemoryRepository(string fullPath)
    {
        if (string.IsNullOrEmpty(fullPath))
        {
            throw new Exception();
        }

        _fs = new MemoryFileSystem();
        FullPath = string.Empty;
        _streamCreator = OpenStream;
        _componentsCreator = GetComponents;
    }

    ~InMemoryRepository() => Dispose(false);

    public string FullPath { get; }

    public Stream OpenStream(string path)
    {
        path = Path.GetFullPath(path);
        if (_fs.FileExists(path))
        {
            _fs.OpenFile(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        }

        return _fs.CreateFile(path);
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
        if (DirectoryExists(partialPath))
        {
            return new FolderComponent(Path.GetFileName(partialPath), () => _componentsCreator(partialPath));
        }

        return new FileComponent(Path.GetFileName(partialPath), () => _streamCreator(partialPath));
    }

    public IReadOnlyCollection<IComponent> GetComponents(string path)
    {
        throw new Exception();
    }

    public IReadOnlyCollection<string> GetRelativePathsOfFolderSubFiles(string path)
    {
        throw new Exception();
    }

    public void CreateDirectory(string path) => _fs.CreateDirectory(Path.GetFullPath(path));

    public bool Exists(string path)
    {
        path = Path.GetFullPath(path);
        return _fs.FileExists(path) || _fs.DirectoryExists(path);
    }

    public bool FileExists(string path)
    {
        path = Path.GetFullPath(path);
        return _fs.FileExists(path);
    }

    public bool DirectoryExists(string path)
    {
        path = Path.GetFullPath(path);
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