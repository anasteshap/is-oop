using Banks.Models;

namespace Banks.Accounts.AccountConfigurations;

public class BankConfiguration
{
    internal BankConfiguration(
        Percent debitPercent,
        Dictionary<Range, Percent> depositPercents,
        Commission creditCommission,
        Limit creditLimit,
        Limit limitForDubiousClient,
        uint depositPeriodInDays)
    {
        DebitPercent = debitPercent;
        DepositPercents = depositPercents;
        CreditCommission = creditCommission;
        CreditLimit = creditLimit;
        LimitForDubiousClient = limitForDubiousClient;
        DepositPeriodInDays = depositPeriodInDays;
    }

    public Percent DebitPercent { get; }
    public Dictionary<Range, Percent> DepositPercents { get; }
    public Commission CreditCommission { get; }
    public Limit CreditLimit { get; }
    public Limit LimitForDubiousClient { get; }
    public uint DepositPeriodInDays { get; }
}