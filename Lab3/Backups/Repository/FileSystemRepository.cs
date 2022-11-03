using Backups.Component;
using Backups.Inter;

namespace Backups.Repository;

public class FileSystemRepository : IRepository
{
    private readonly List<IComponent> _components = new ();
    public FileSystemRepository(string pathName)
    {
        if (string.IsNullOrEmpty(pathName))
        {
            throw new Exception();
        }

        FullName = pathName;
        foreach (string obj in GetRelativePathsOfFolderSubFiles(FullName).ToList())
        {
            if (DirectoryExists(obj))
            {
                _components.Add(new FolderComponent(this, obj));
            }
            else
            {
                _components.Add(new FileComponent(this, obj));
            }
        }
    }

    public string FullName { get; }

    public Stream OpenStream(string path) => new FileStream(Path.GetFullPath(path, FullName), FileMode.OpenOrCreate, FileAccess.ReadWrite);

    public void SaveTo(string path, Stream stream)
    {
        CreateDirectory(path);
        var newStream = new FileStream(Path.GetFullPath(path, FullName), FileMode.OpenOrCreate, FileAccess.Write);
        stream.CopyTo(newStream);
    }

    public IReadOnlyCollection<IComponent> Components() => _components;

    public IRepository GetSubRepository(string partialPath) => new FileSystemRepository($"{FullName}/{partialPath}");

    public IReadOnlyCollection<string> GetRelativePathsOfFolderSubFiles(string path)
    {
        if (!Directory.Exists(Path.GetFullPath(path, FullName)))
        {
            throw new Exception();
        }

        var list = Directory.GetFileSystemEntries(Path.GetFullPath(path, $"{FullName}/")).ToList();
        var newList = new List<string>();
        foreach (string a in list)
        {
            newList.Add(Path.GetRelativePath(FullName, a));
        }

        return newList;
    }

    public void CreateDirectory(string path)
    {
        var folder = Path.GetDirectoryName(Path.GetFullPath(path, FullName));
        if (!string.IsNullOrEmpty(folder) && !DirectoryExists(folder))
        {
            Directory.CreateDirectory(folder);
        }
    }

    public bool Exists(string path)
    {
        path = Path.GetFullPath(path, FullName);
        return Directory.Exists(path) || File.Exists(path);
    }

    public bool FileExists(string path)
    {
        path = Path.GetFullPath(path, FullName);
        return File.Exists(path);
    }

    public bool DirectoryExists(string path)
    {
        path = Path.GetFullPath(path, FullName);
        return Directory.Exists(path);
    }
}