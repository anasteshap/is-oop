using Backups.Repository;

namespace Backups.Inter;

public interface IBackupObject
{
    public string FullName { get; }
    public IRepository Repository { get; }
}