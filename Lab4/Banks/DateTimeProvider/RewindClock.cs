namespace Banks.DateTimeProvider;

public class RewindClock : IClock
{
    private TimeSpan _timeSpan = TimeSpan.Zero;
    private event Action? Rewind;
    public DateTime CurrentTime() => DateTime.Now.Add(_timeSpan);

    public void RewindTime(TimeSpan timeSpan)
    {
        if (timeSpan < TimeSpan.Zero)
            throw new Exception("Выберите другой интервал для перемотки времени");

        TimeSpan newTimeSpan = _timeSpan + timeSpan;
        for (int i = 0; i < newTimeSpan.Days; i++)
        {
            _timeSpan += new TimeSpan(1, 0, 0, 0);
            Rewind?.Invoke();
        }

        // _timeSpan = newTimeSpan;
    }

    public void AddAction(Action action) => Rewind += action;

    public void RemoveAction(Action action) => Rewind -= action;
}