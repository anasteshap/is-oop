namespace Shops.Exceptions;

public class ShopException : Exception
{
    private ShopException(string message)
        : base(message) { }

    public static ShopException InvalidName()
    {
        return new ShopException("Name or address is invalid (null)");
    }

    public static ShopException InvalidPrice(decimal price)
    {
        return new ShopException($"{price} - invalid price of product");
    }

    public static ShopException InvalidAmount(int amount)
    {
        return new ShopException($"{amount} - invalid amount of product");
    }
}