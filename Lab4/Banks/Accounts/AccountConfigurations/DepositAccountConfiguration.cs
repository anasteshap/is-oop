namespace Banks.Accounts.AccountConfigurations;

public class DepositAccountConfiguration
{
    private List<DepositPercent> _depositPercents;
    public DepositAccountConfiguration(List<DepositPercent> depositPercents, TimeSpan time)
    {
        _depositPercents = depositPercents;
        Time = time;
    }

    public IReadOnlyCollection<DepositPercent> DepositPercents => _depositPercents;
    public TimeSpan Time { get; private set; }
    public void SetDepositPercents(List<DepositPercent> percents)
    {
        if (percents.Count == 0)
            throw new Exception();
        if (percents[0].LeftBorder != 0)
            throw new Exception();

        for (int i = 0; i < _depositPercents.Count - 1; i++)
        {
            if (_depositPercents[i].RightBorder != _depositPercents[i + 1].LeftBorder)
                throw new Exception();
        }

        _depositPercents = percents;
    }

    public void SetTimeSpan(TimeSpan time)
    {
        if (time <= TimeSpan.Zero)
            throw new Exception();
        Time = time;
    }
}