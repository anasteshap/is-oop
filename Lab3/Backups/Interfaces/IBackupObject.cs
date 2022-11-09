using Backups.Component;
using Backups.Repository;

namespace Backups.Interfaces;

public interface IBackupObject
{
    string RelativePath { get; }
    IRepository Repository { get; }
    IComponent GetRepoComponent();
}