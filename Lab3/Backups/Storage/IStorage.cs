using Backups.Component;

namespace Backups.Storage;

public interface IStorage
{
    IReadOnlyCollection<IComponent> GetRepoComponents();
}