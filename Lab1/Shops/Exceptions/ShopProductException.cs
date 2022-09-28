namespace Shops.Exceptions;

public class ShopProductException : Exception
{
    private ShopProductException(string message)
        : base(message) { }

    public static ShopProductException InvalidName()
    {
        return new ShopProductException("Name is invalid (null)");
    }

    public static ShopProductException InvalidPrice(decimal price)
    {
        return new ShopProductException($"{price} - invalid price of product");
    }

    public static ShopProductException InvalidAmount(int amount)
    {
        return new ShopProductException($"{amount} - invalid amount of product");
    }

    public static ShopProductException InvalidProduct()
    {
        return new ShopProductException("Product is invalid (null)");
    }

    public static ShopProductException ProductDoesNotExist(string name)
    {
        return new ShopProductException($"Product {name} doesn't exist");
    }

    public static ShopProductException NotEnoughAmount(string name)
    {
        return new ShopProductException($"There isn't enough products ({name}) in shop in shop");
    }
}