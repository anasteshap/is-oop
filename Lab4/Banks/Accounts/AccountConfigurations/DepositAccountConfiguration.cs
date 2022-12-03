using Banks.Exceptions;

namespace Banks.Accounts.AccountConfigurations;

public class DepositAccountConfiguration
{
    private List<DepositPercent> _depositPercents;
    public DepositAccountConfiguration(List<DepositPercent> depositPercents, TimeSpan time)
    {
        ValidateDepositPercents(depositPercents);
        ArgumentNullException.ThrowIfNull(nameof(time));
        _depositPercents = depositPercents;
        Time = time;
    }

    public IReadOnlyCollection<DepositPercent> DepositPercents => _depositPercents;
    public TimeSpan Time { get; private set; }
    public void SetDepositPercents(List<DepositPercent> percents)
    {
        ValidateDepositPercents(percents);
        _depositPercents = percents;
    }

    public void SetTimeSpan(TimeSpan time)
    {
        if (time <= TimeSpan.Zero)
            throw AccountException.InvalidPeriodOfAccount();
        Time = time;
    }

    private void ValidateDepositPercents(List<DepositPercent> depositPercents)
    {
        if (depositPercents.Count == 0)
            throw AccountException.InvalidConfiguration("No deposit interest");
        if (depositPercents[0].LeftBorder != 0)
            throw AccountException.InvalidConfiguration("LeftBorder of first depositPercent must be equal to 0");

        for (int i = 0; i < _depositPercents.Count - 1; i++)
        {
            if (_depositPercents[i].RightBorder != _depositPercents[i + 1].LeftBorder)
                throw AccountException.InvalidConfiguration("LeftBorder of first depositPercent and rightBorder of next one must be equal");
            if (_depositPercents[i].Percent.Value > _depositPercents[i + 1].Percent.Value)
                throw AccountException.InvalidConfiguration("First depositPercent must be <= next depositPercent");
        }
    }
}