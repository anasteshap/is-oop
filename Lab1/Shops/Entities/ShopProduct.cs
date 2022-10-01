using System.ComponentModel;
using Shops.Exceptions;

namespace Shops.Entities;

public class ShopProduct
{
    public ShopProduct(Product product, int amount, decimal price)
    {
        if (amount < 0)
        {
            throw ShopProductException.InvalidAmount(amount);
        }

        if (price <= 0)
        {
            throw ShopProductException.InvalidPrice(price);
        }

        Product = product;
        Amount = amount;
        Price = price;
    }

    public Product Product { get; }
    public int Amount { get; private set; }

    public decimal Price { get; private set; }

    public void ChangePrice(decimal newPrice)
    {
        if (newPrice <= 0)
        {
            throw ShopProductException.InvalidPrice(newPrice);
        }

        Price = newPrice;
    }

    public void IncreaseAmount(int newAmount)
    {
        if (newAmount < 0)
        {
            throw ShopProductException.InvalidAmount(newAmount);
        }

        Amount += newAmount;
    }

    public void ReduceAmount(int newAmount)
    {
        if (Amount < newAmount)
        {
            throw ShopProductException.InvalidAmount(Amount - newAmount);
        }

        Amount -= newAmount;
    }
}