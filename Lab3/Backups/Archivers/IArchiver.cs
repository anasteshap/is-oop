using System.Collections.ObjectModel;
using Backups.Component;
using Backups.Repository;

namespace Backups.Archivers;

public interface IArchiver
{
    // в зависимости от типа алгоритма кидает все объекты сразу или по одному
    void Archive(List<IComponent> components, IRepository repository, string zipPath); // тут репозиторий из таски
}