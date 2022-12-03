using System.Globalization;
using Banks.Accounts.AccountConfigurations;
using Banks.DateTimeProvider;
using Banks.Entities;
using Banks.Interfaces;
using Banks.Models;

namespace Banks.Accounts;

public class DebitAccount : BaseAccount
{
    private readonly DebitAccountConfiguration _configuration;
    private readonly Limit _limitForDubiousClient;
    private readonly IClock _clock;
    private decimal _percentageAmount = 0;
    private int _countOfDays;
    internal DebitAccount(IClock clock, IClient client, BankConfiguration bankConfiguration)
        : base(client, TypeOfBankAccount.Debit)
    {
        _clock = clock;
        _countOfDays = new GregorianCalendar().GetDaysInMonth(_clock.CurrentTime().Year, _clock.CurrentTime().Month);
        _configuration = bankConfiguration.DebitAccountConfiguration;
        _limitForDubiousClient = bankConfiguration.LimitForDubiousClient;
        Balance = 0;
        clock.AddAction(AccountDailyPayoff);
    }

    public override void AccountDailyPayoff() // internal
    {
        int daysInYear = new GregorianCalendar().GetDaysInYear(_clock.CurrentTime().Year);
        _percentageAmount += (Balance * _configuration.DebitPercent.Value) / daysInYear;
        _countOfDays--;
        if (_countOfDays == 0)
        {
            Balance += _percentageAmount;
            _percentageAmount = 0;
            _countOfDays = new GregorianCalendar().GetDaysInMonth(_clock.CurrentTime().Year, _clock.CurrentTime().Month);
        }
    }

    public override void DecreaseAmount(decimal sum)
    {
        if (sum <= 0)
            throw new Exception("Sum can't be <= 0");

        if (Balance < sum)
            throw new Exception("Sum > balance");

        if (Client.IsDubious)
        {
            if (sum > _limitForDubiousClient.Value)
                throw new Exception($"You can't withdraw money more than limit {_limitForDubiousClient.Value}");
        }

        Balance -= sum;
    }
}