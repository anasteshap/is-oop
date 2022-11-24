using Banks.Accounts.AccountConfigurations;
using Banks.Interfaces;

namespace Banks.Accounts.Factory;

internal class AccountFactory
{
    /*internal static BaseAccount Create(TypeOfBankAccount typeOfBankAccount, IClient client, decimal amount, BankConfiguration bankConfiguration)
    {
        return typeOfBankAccount switch
        {
            TypeOfBankAccount.Credit => new CreditAccount(client, amount, bankConfiguration),
            TypeOfBankAccount.Debit => new DebitAccount(client, amount, bankConfiguration),
            TypeOfBankAccount.Deposit => new DepositAccount(client, amount, bankConfiguration),
            _ => throw new ArgumentOutOfRangeException(nameof(typeOfBankAccount), typeOfBankAccount, null)
        };
    }*/
    internal static BaseAccount CreateCreditAccount(IClient client, decimal amount, BankConfiguration bankConfiguration)
    {
        return new CreditAccount(client, amount, bankConfiguration);
    }

    internal static BaseAccount CreateDebitAccount(IClient client, decimal amount, BankConfiguration bankConfiguration)
    {
        return new DebitAccount(client, amount, bankConfiguration);
    }

    internal static BaseAccount CreateDepositAccount(IClient client, decimal amount, BankConfiguration bankConfiguration)
    {
        return new DepositAccount(client, amount, bankConfiguration);
    }
}