using Banks.Models;

namespace Banks.Accounts.AccountConfigurations;

public class BankConfiguration
{
    internal BankConfiguration(
        Percent debitPercent,
        Percent depositPercent,
        Commission creditCommission,
        Limit creditLimit,
        Limit limitForDubiousClient,
        uint depositPeriodInDays)
    {
        DebitPercent = debitPercent;
        DepositPercent = depositPercent;
        CreditCommission = creditCommission;
        CreditLimit = creditLimit;
        LimitForDubiousClient = limitForDubiousClient;
        DepositPeriodInDays = depositPeriodInDays;
    }

    public Percent DebitPercent { get; }
    public Percent DepositPercent { get; }
    public Commission CreditCommission { get; }
    public Limit CreditLimit { get; }
    public Limit LimitForDubiousClient { get; }
    public uint DepositPeriodInDays { get; }
}