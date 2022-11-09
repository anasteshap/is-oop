using System.Collections.ObjectModel;
using Backups.Component;
using Backups.Inter;

namespace Backups.Repository;

public class FileSystemRepository : IRepository
{
    private readonly Func<string, Stream> _streamCreator;
    private readonly Func<string, IReadOnlyCollection<IComponent>> _componentsCreator;

    public FileSystemRepository(string pathName)
    {
        if (string.IsNullOrEmpty(pathName))
        {
            throw new Exception();
        }

        FullName = pathName;
        _streamCreator = OpenStream;
        _componentsCreator = GetComponents;
    }

    public string FullName { get; }

    public Stream OpenStream(string path) =>
        new FileStream(Path.GetFullPath(path, FullName), FileMode.OpenOrCreate, FileAccess.ReadWrite);

    public void SaveTo(string path, Stream stream)
    {
        CreateDirectory(path);
        var newStream = new FileStream(Path.GetFullPath(path, FullName), FileMode.OpenOrCreate, FileAccess.Write);
        stream.CopyTo(newStream);
    }

    public IRepository GetSubRepository(string partialPath) => new FileSystemRepository($"{FullName}/{partialPath}");

    public IComponent GetRepositoryComponent(string partialPath)
    {
        if (DirectoryExists(partialPath))
        {
            return new FolderComponent(partialPath, _componentsCreator);
        }

        return new FileComponent(partialPath, _streamCreator);
    }

    public IReadOnlyCollection<IComponent> GetComponents(string path)
    {
        var relativePaths = GetRelativePathsOfFolderSubFiles(path).ToList();
        var directories = relativePaths
            .Where(DirectoryExists)
            .Select(x => new FolderComponent(x, _componentsCreator))
            .ToList();
        var files = relativePaths
            .Where(FileExists)
            .Select(x => new FileComponent(x, _streamCreator))
            .ToList();

        return new List<IComponent>().Concat(directories).Concat(files).ToList();
    }

    public IReadOnlyCollection<string> GetRelativePathsOfFolderSubFiles(string path)
    {
        if (!Directory.Exists(Path.GetFullPath(path, FullName)))
        {
            throw new Exception();
        }

        var files = Directory.GetFiles(Path.GetFullPath(path, $"{FullName}")).ToList();
        var directories = Directory.GetDirectories(Path.GetFullPath(path, $"{FullName}")).ToList();
        var infos = files.Concat(directories).ToList();
        var newList = new List<string>();
        foreach (string a in infos)
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