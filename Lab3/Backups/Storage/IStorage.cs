using Backups.Component;

namespace Backups.Storage;

public interface IStorage
{
    int GetZipArchivesCount();
    IReadOnlyCollection<IComponent> GetRepoComponents();
}