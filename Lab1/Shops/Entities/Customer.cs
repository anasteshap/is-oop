using Shops.Exceptions;

namespace Shops.Entities;

public class Customer
{
    public Customer(string name, decimal score = 0)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw CustomerException.InvalidName();
        }

        if (score < 0)
        {
            throw CustomerException.InvalidScore(score);
        }

        Name = name;
        Score = score;
        Id = Guid.NewGuid();
    }

    public string Name { get; }
    public decimal Score { get; private set; }
    public Guid Id { get; }

    public void CheckPriceValidity(decimal price)
    {
        if (price <= 0)
        {
            throw ShopProductException.InvalidPrice(price);
        }

        if (Score < price)
        {
            throw CustomerException.NotEnoughMoney(price);
        }
    }

    public void SpendMoney(decimal price)
    {
        CheckPriceValidity(price);
        Score -= price;
    }

    public void GetMoney(decimal price)
    {
        CheckPriceValidity(price);
        Score += price;
    }
}