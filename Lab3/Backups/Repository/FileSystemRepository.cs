using System.Collections.ObjectModel;
using Backups.Component;

namespace Backups.Repository;

public class FileSystemRepository : IRepository
{
    private readonly Func<string, Stream> _streamCreator;
    private readonly Func<string, IReadOnlyCollection<IComponent>> _componentsCreator;

    public FileSystemRepository(string fullPath)
    {
        if (string.IsNullOrEmpty(fullPath))
        {
            throw new Exception();
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
        CreateDirectory(path);
        var newStream = new FileStream(Path.GetFullPath(path, FullPath), FileMode.OpenOrCreate, FileAccess.Write);
        stream.CopyTo(newStream);
    }

    public IRepository GetSubRepository(string partialPath) => new FileSystemRepository($"{FullPath}/{partialPath}");

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
        var relativePaths = GetRelativePathsOfFolderSubFiles(path).ToList();
        var directories = relativePaths
            .Where(DirectoryExists)
            .Select(x => new FolderComponent(Path.GetFileName(x), () => _componentsCreator(x)))
            .ToList();
        var files = relativePaths
            .Where(FileExists)
            .Select(x => new FileComponent(Path.GetFileName(x), () => _streamCreator(x)))
            .ToList();

        return new List<IComponent>().Concat(directories).Concat(files).ToList();
    }

    public IReadOnlyCollection<string> GetRelativePathsOfFolderSubFiles(string path)
    {
        if (!Directory.Exists(Path.GetFullPath(path, FullPath)))
        {
            throw new Exception();
        }

        var files = Directory.GetFiles(Path.GetFullPath(path, $"{FullPath}")).ToList();
        var directories = Directory.GetDirectories(Path.GetFullPath(path, $"{FullPath}")).ToList();
        var infos = files.Concat(directories).ToList();
        var newList = new List<string>();
        foreach (string a in infos)
        {
            newList.Add(Path.GetRelativePath(FullPath, a));
        }

        return newList;
    }

    public void CreateDirectory(string path)
    {
        var folder = Path.GetDirectoryName(Path.GetFullPath(path, FullPath));
        if (!string.IsNullOrEmpty(folder) && !DirectoryExists(folder))
        {
            Directory.CreateDirectory(folder);
        }
    }

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