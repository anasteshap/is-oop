namespace Banks.Exceptions;

public class AccountException : Exception
{
    private AccountException(string message)
        : base(message) { }

    public static AccountException InvalidConfiguration(string message)
    {
        return new AccountException(message);
    }

    public static AccountException NotEnoughMoney()
    {
        return new AccountException("Don't have enough money on account");
    }

    public static AccountException InvalidPeriodOfAccount()
    {
        return new AccountException("Period of the account is invalid");
    }

    public static AccountException Expiration(Guid id)
    {
        return new AccountException($"Account with id: {id.ToString()} expired");
    }

    public static AccountException AccountAlreadyExists(Guid id)
    {
        return new AccountException($"Account with id: {id.ToString()} already exists");
    }

    public static AccountException AccountDoesNotExist(Guid id)
    {
        return new AccountException($"Account with id: {id.ToString()} doesn't exist");
    }
}