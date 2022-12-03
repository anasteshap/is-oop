using Banks.Models;

namespace Banks.Accounts.AccountConfigurations;

public class CreditAccountConfiguration
{
    public CreditAccountConfiguration(decimal creditCommission, decimal creditLimit)
    {
        CreditCommission = creditCommission;
        CreditLimit = creditLimit;
    }

    public decimal CreditCommission { get; private set; }
    public decimal CreditLimit { get; private set; }

    public void SetCreditCommission(decimal commission)
    {
        if (commission < 0)
            throw new Exception();
        CreditCommission = commission;
    }

    public void SetCreditLimit(decimal limit)
    {
        if (limit < 0)
            throw new Exception();
        CreditLimit = limit;
    }
}