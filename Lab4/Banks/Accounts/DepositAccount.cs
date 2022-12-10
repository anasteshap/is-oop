using System.Globalization;
using Banks.Accounts.AccountConfigurations;
using Banks.DateTimeProvider;
using Banks.Exceptions;
using Banks.Interfaces;
using Banks.Models;

namespace Banks.Accounts;

public class DepositAccount : BaseAccount
{
    private readonly DepositAccountConfiguration _configuration;
    private readonly Limit _limitForDubiousClient;
    private readonly DateTime _endOfPeriod;
    private readonly IClock _clock;
    private decimal _percentageAmount = 0;
    internal DepositAccount(IClock clock, IClient client, BankConfiguration bankConfiguration, TimeSpan? endOfPeriod = null)
        : base(client, TypeOfBankAccount.Deposit)
    {
        ArgumentNullException.ThrowIfNull(nameof(clock));
        ArgumentNullException.ThrowIfNull(nameof(bankConfiguration));
        _clock = clock;
        _configuration = bankConfiguration.DepositAccountConfiguration;
        _limitForDubiousClient = bankConfiguration.LimitForDubiousClient;
        _endOfPeriod = (endOfPeriod is not null)
            ? DateTime.Now + endOfPeriod.Value
            : DateTime.Now + bankConfiguration.DepositAccountConfiguration.Time;
        clock.AddAction(AccountDailyPayoff);
        Balance = 0;
    }

    public override void AccountDailyPayoff()
    {
        if (_clock.CurrentTime().Date > _endOfPeriod.Date)
            throw AccountException.Expiration(Id);

        if (_clock.CurrentTime().Date == _endOfPeriod.Date)
        {
            Balance += _percentageAmount;
            _percentageAmount = 0;
            return;
        }

        Percent percent = _configuration.DepositPercents
            .Single(x => x.LeftBorder <= Balance && Balance < x.RightBorder)
            .Percent;
        int daysInYear = new GregorianCalendar().GetDaysInYear(_clock.CurrentTime().Year);
        _percentageAmount += (Balance * percent.Value * 100) / daysInYear;
    }

    public override void DecreaseAmount(decimal sum)
    {
        if (sum <= 0)
            throw TransactionException.NegativeAmount();

        if (_clock.CurrentTime().Date < _endOfPeriod.Date)
            throw AccountException.Expiration(Id);

        if (Client.IsDubious)
        {
            if (sum > _limitForDubiousClient.Value)
                throw TransactionException.SumExceedingLimit(sum, _limitForDubiousClient.Value);
        }

        if (Balance < sum)
            throw AccountException.NotEnoughMoney();

        Balance -= sum;
    }
}