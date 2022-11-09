using Backups.Component;

namespace Backups.Storage;

public interface IStorage
{
    IComponent GetRepoComponents();
}