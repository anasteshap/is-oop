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
        CreditAccountConfiguration = creditAccountConfiguration ?? throw new ArgumentNullException();
        DebitAccountConfiguration = debitAccountConfiguration ?? throw new ArgumentNullException();
        DepositAccountConfiguration = depositAccountConfiguration ?? throw new ArgumentNullException();
        LimitForDubiousClient = limitForDubiousClient ?? throw new ArgumentNullException();
    }

    public CreditAccountConfiguration CreditAccountConfiguration { get; }
    public DebitAccountConfiguration DebitAccountConfiguration { get; }
    public DepositAccountConfiguration DepositAccountConfiguration { get; }
    public Limit LimitForDubiousClient { get; private set; }
    public void SetLimitForDubiousClient(decimal limit) => LimitForDubiousClient = new Limit(limit);
}