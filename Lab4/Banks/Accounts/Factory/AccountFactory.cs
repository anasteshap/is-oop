using Banks.Accounts.AccountConfigurations;
using Banks.Interfaces;

namespace Banks.Accounts.Factory;

internal class AccountFactory
{
    internal static BaseAccount CreateCreditAccount(IClient client, BankConfiguration bankConfiguration)
    {
        return new CreditAccount(client, bankConfiguration);
    }

    internal static BaseAccount CreateDebitAccount(IClient client, BankConfiguration bankConfiguration)
    {
        return new DebitAccount(client, bankConfiguration);
    }

    internal static BaseAccount CreateDepositAccount(IClient client, BankConfiguration bankConfiguration, uint? periodInDays)
    {
        return new DepositAccount(client, bankConfiguration, periodInDays);
    }
}