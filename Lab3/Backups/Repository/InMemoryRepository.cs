using Backups.Component;
using Backups.Inter;
using Zio;
using Zio.FileSystems;

namespace Backups.Repository;

public class InMemoryRepository : IRepository, IDisposable
{
    private readonly MemoryFileSystem _fs;
    private bool _disposed;

    public InMemoryRepository(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new Exception();
        }

        _fs = new MemoryFileSystem();
        _fs.Name = name;
        FullName = name;
    }

    ~InMemoryRepository() => Dispose(false);

    public string FullName { get; }

    public Stream OpenStream(string path)
    {
        string fullPath = Path.GetFullPath(path);
        _fs.CreateDirectory(path);
        using Stream stream = _fs.CreateFile($"{path}/1.txt");
        return stream;

        // return new Stream(path, true);
    }

    public void SaveTo(string path, Stream stream)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<IComponent> Components()
    {
        throw new NotImplementedException();
    }

    public IRepository GetSubRepository(string partialPath)
    {
        throw new NotImplementedException();

        /*var a = new SubFileSystem(_fs, partialPath);
        a.
        var newFs = new InMemoryRepository();
        newFs.FullName = FullName*/
    }

    public IReadOnlyCollection<string> GetRelativePathsOfFolderSubFiles(string path)
    {
        throw new NotImplementedException();
    }

    public void CreateDirectory(string path)
    {
        throw new NotImplementedException();
    }

    public bool Exists(string path) => _fs.FileExists(path) || _fs.DirectoryExists(path);

    public bool FileExists(string path) => _fs.FileExists(path);

    public bool DirectoryExists(string path) => _fs.DirectoryExists(path);

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