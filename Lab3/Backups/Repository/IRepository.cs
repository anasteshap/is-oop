using Backups.Component;
using Backups.Inter;

namespace Backups.Repository;

public interface IRepository
{
    string FullName { get; }
    Stream OpenStream(string path);
    void SaveTo(string path, Stream stream);
    IReadOnlyCollection<IComponent> Components();
    IRepository GetSubRepository(string partialPath);

    IReadOnlyCollection<string> GetRelativePathsOfFolderSubFiles(string path);
    void CreateDirectory(string path);
    bool Exists(string path);
    bool FileExists(string path);
    bool DirectoryExists(string path);
}