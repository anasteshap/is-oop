using System.Collections.ObjectModel;
using Backups.Component;
using Backups.Repository;
using Backups.Storage;

namespace Backups.Archivers;

public interface IArchiver
{
    // в зависимости от типа алгоритма кидает все объекты сразу или по одному
    ZipStorage Archive(List<IComponent> components, IRepository repository, string zipPath);
}