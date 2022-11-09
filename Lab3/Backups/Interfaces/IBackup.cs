using Backups.Entities;

namespace Backups.Inter;

public interface IBackup
{
    IReadOnlyCollection<RestorePoint> RestorePoints();
    void AddRestorePoint(RestorePoint restorePoint);
    void DeleteRestorePoint(RestorePoint restorePoint);
}