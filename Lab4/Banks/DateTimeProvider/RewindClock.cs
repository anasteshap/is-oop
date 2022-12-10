using Banks.Exceptions;

namespace Banks.DateTimeProvider;

public class RewindClock : IClock
{
    private TimeSpan _timeSpan = TimeSpan.Zero;
    private event Action? Rewind;
    public DateTime CurrentTime() => DateTime.Now.Add(_timeSpan);

    public void RewindTime(TimeSpan timeSpan)
    {
        if (timeSpan < TimeSpan.Zero)
            throw AccountException.InvalidPeriodOfAccount();

        TimeSpan newTimeSpan = _timeSpan + timeSpan;
        for (int i = 0; i < newTimeSpan.Days; i++)
        {
            _timeSpan += new TimeSpan(1, 0, 0, 0);
            Rewind?.Invoke();
        }
    }

    public void AddAction(Action action) => Rewind += action;
}