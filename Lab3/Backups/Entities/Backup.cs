using Backups.Interfaces;

namespace Backups.Entities;

public class Backup : IBackup
{
    private List<RestorePoint> _restorePoints = new ();
    public Backup() { }

    public IReadOnlyCollection<RestorePoint> RestorePoints() => _restorePoints;

    public void AddRestorePoint(RestorePoint restorePoint)
    {
        if (_restorePoints.Contains(restorePoint))
        {
            throw new Exception();
        }

        _restorePoints.Add(restorePoint);
    }

    public void DeleteRestorePoint(RestorePoint restorePoint)
    {
        if (!_restorePoints.Remove(restorePoint))
        {
            throw new Exception();
        }
    }
}