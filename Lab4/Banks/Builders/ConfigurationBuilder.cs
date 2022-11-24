using Banks.Accounts.AccountConfigurations;
using Banks.Models;

namespace Banks.Builders;

public class ConfigurationBuilder
{
    private Percent? _debitPercent;
    private Percent? _depositPercent;
    private Commission? _creditCommission;
    private Limit? _creditLimit;
    private Limit? _limitForDubiousClient;
    private uint? _depositPeriodInDays;

    public ConfigurationBuilder()
    {
        _debitPercent = null;
        _depositPercent = null;
        _creditCommission = null;
        _creditLimit = null;
        _limitForDubiousClient = null;
        _depositPeriodInDays = null;
    }

    public ConfigurationBuilder AddDebitPercent(Percent debitPercent)
    {
        _debitPercent = debitPercent;
        return this;
    }

    public ConfigurationBuilder AddDepositPercent(Percent depositPercent)
    {
        _depositPercent = depositPercent;
        return this;
    }

    public ConfigurationBuilder AddCommission(Commission creditCommission)
    {
        _creditCommission = creditCommission;
        return this;
    }

    public ConfigurationBuilder AddCreditLimit(Limit creditLimit)
    {
        _creditLimit = creditLimit;
        return this;
    }

    public ConfigurationBuilder AddLimitForDubiousClient(Limit limitForDubiousClient)
    {
        _limitForDubiousClient = limitForDubiousClient;
        return this;
    }

    public ConfigurationBuilder AddDepositPeriodInDays(uint depositPeriodInDays)
    {
        _depositPeriodInDays = depositPeriodInDays;
        return this;
    }

    public BankConfiguration Build()
    {
        return new BankConfiguration(
            _debitPercent ?? throw new ArgumentNullException(),
            _depositPercent ?? throw new ArgumentNullException(),
            _creditCommission ?? throw new ArgumentNullException(),
            _creditLimit ?? throw new ArgumentNullException(),
            _limitForDubiousClient ?? throw new ArgumentNullException(),
            _depositPeriodInDays ?? throw new ArgumentNullException());
    }
}