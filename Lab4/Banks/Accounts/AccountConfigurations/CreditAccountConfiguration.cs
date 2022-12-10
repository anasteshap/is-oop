using Banks.Exceptions;
using Banks.Models;

namespace Banks.Accounts.AccountConfigurations;

public class CreditAccountConfiguration
{
    public CreditAccountConfiguration(decimal creditCommission, decimal creditLimit)
    {
        if (creditCommission < 0 || creditLimit <= 0)
            throw AccountException.InvalidConfiguration($"Your creditAccountConfiguration: commission = {creditCommission}, limit = {creditLimit}");
        CreditCommission = creditCommission;
        CreditLimit = creditLimit;
    }

    public decimal CreditCommission { get; private set; }
    public decimal CreditLimit { get; private set; }

    public void SetCreditCommission(decimal commission)
    {
        if (commission < 0)
            throw AccountException.InvalidConfiguration("CreditCommission < 0");
        CreditCommission = commission;
    }

    public void SetCreditLimit(decimal limit)
    {
        if (limit < 0)
            throw AccountException.InvalidConfiguration("CreditLimit < 0");
        CreditLimit = limit;
    }
}