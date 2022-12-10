using Banks.Models;

namespace Banks.Accounts.AccountConfigurations;

public class BankConfiguration
{
    public BankConfiguration(
        CreditAccountConfiguration creditAccountConfiguration,
        DebitAccountConfiguration debitAccountConfiguration,
        DepositAccountConfiguration depositAccountConfiguration,
        Limit limitForDubiousClient)
    {
        ArgumentNullException.ThrowIfNull(nameof(creditAccountConfiguration));
        ArgumentNullException.ThrowIfNull(nameof(debitAccountConfiguration));
        ArgumentNullException.ThrowIfNull(nameof(depositAccountConfiguration));
        ArgumentNullException.ThrowIfNull(nameof(limitForDubiousClient));
        CreditAccountConfiguration = creditAccountConfiguration;
        DebitAccountConfiguration = debitAccountConfiguration;
        DepositAccountConfiguration = depositAccountConfiguration;
        LimitForDubiousClient = limitForDubiousClient;
    }

    public CreditAccountConfiguration CreditAccountConfiguration { get; }
    public DebitAccountConfiguration DebitAccountConfiguration { get; }
    public DepositAccountConfiguration DepositAccountConfiguration { get; }
    public Limit LimitForDubiousClient { get; private set; }
    public void SetLimitForDubiousClient(decimal limit) => LimitForDubiousClient = new Limit(limit);
}