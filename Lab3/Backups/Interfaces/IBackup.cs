using Backups.Entities;

namespace Backups.Interfaces;

public interface IBackup
{
    IReadOnlyCollection<RestorePoint> RestorePoints { get; }
    void AddRestorePoint(RestorePoint restorePoint);
    void DeleteRestorePoint(RestorePoint restorePoint);
}