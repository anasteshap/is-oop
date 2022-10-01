using Shops.Entities;

namespace Shops.Exceptions;

public class CustomerException : Exception
{
    private CustomerException(string message)
        : base(message) { }

    public static CustomerException InvalidName()
    {
        return new CustomerException("Name is invalid (null)");
    }

    public static CustomerException InvalidScore(decimal score)
    {
        return new CustomerException($"Customer can't have less than 0 money ({score} < 0)");
    }

    public static CustomerException NotEnoughMoney(decimal price)
    {
        return new CustomerException($"Customer can't buy the product for this price ({price})");
    }

    public static CustomerException CustomerDoesNotExist()
    {
        return new CustomerException($"Customer doesn't exist (null)");
    }

    public static CustomerException CustomerDoesNotExist(Customer customer)
    {
        return new CustomerException($"Customer {customer.Name} with id = {customer.Id} doesn't exist");
    }
}