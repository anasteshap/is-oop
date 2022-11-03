using Backups.Inter;

namespace Backups.Entities;

/*public class FileSystemRepository : IRepository, IRepoComponent
{
    private DirectoryInfo _directoryInfo;
    public FileSystemRepository(string pathName)
    {
        if (string.IsNullOrEmpty(pathName))
        {
            throw new Exception();
        }

        _directoryInfo = new DirectoryInfo(pathName);
        _directoryInfo.Create();
        FullName = _directoryInfo.FullName;
        Name = _directoryInfo.Name;
    }

    public string FullName { get; }
    public string Name { get; }
    public DirectoryInfo GetDirectoryInfo() => _directoryInfo;
    public bool Exists(string objectName)
    {
        // var objects = Directory.GetFileSystemEntries(FullName);
        var objects = _directoryInfo.GetFileSystemInfos();
        return objects.FirstOrDefault(x => x.Name == objectName) is not null;
    }

    public void AddBackupTask(IBackupTask backupTask)
    {
        _directoryInfo.CreateSubdirectory(backupTask.Name);
    }

    public DateTime CreateDirectoryByPath(string path)
    {
        return Directory.CreateDirectory(path).CreationTime;
    }

    public void Add(IRepoComponent repoComponent)
    {
        if (repoComponent is null)
        {
            throw new Exception();
        }

        if (repoComponent is FolderComponent && !Directory.Exists(repoComponent.FullName))
        {
            throw new Exception();
        }

        if (repoComponent is FileComponent && !File.Exists(repoComponent.FullName))
        {
            throw new Exception();
        }

        // или перезаписывать файлы с одинаковым названием?
        /* if (Exists(Path.GetFileName(repoComponent.FullName)))
        {
            throw new Exception();
        }

        repoComponent.AddInRepo(FullName);
    }
}*/