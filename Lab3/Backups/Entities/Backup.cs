using Backups.Exceptions;
using Backups.Interfaces;

namespace Backups.Entities;

public class Backup : IBackup
{
    private readonly List<RestorePoint> _restorePoints = new ();

    public IReadOnlyCollection<RestorePoint> RestorePoints => _restorePoints;

    public void AddRestorePoint(RestorePoint restorePoint)
    {
        if (_restorePoints.Contains(restorePoint))
        {
            throw BackupException.RestorePointAlreadyExists(restorePoint.RelativePath);
        }

        _restorePoints.Add(restorePoint);
    }

    public void DeleteRestorePoint(RestorePoint restorePoint)
    {
        if (!_restorePoints.Remove(restorePoint))
        {
            throw BackupException.RestorePointDoesNotExist(restorePoint.RelativePath);
        }
    }
}