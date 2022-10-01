using Shops.Entities;

namespace Shops.Exceptions;

public class ShopServiceException : Exception
{
    private ShopServiceException(string message)
        : base(message) { }

    public static ShopServiceException ProductAlreadyRegistered(Product product)
    {
        return new ShopServiceException($"Product {product.Name} has already registered");
    }

    public static ShopServiceException ProductDoesNotRegistered(Product product)
    {
        return new ShopServiceException($"Product {product.Name} doesn't registered");
    }

    public static ShopServiceException ShopAlreadyExist(Shop shop)
    {
        return new ShopServiceException($"{shop.Name} ({shop.ShopAddress}) has already existed");
    }

    public static ShopServiceException ShopDoesNotExist()
    {
        return new ShopServiceException($"Shop doesn't exist (null)");
    }

    public static ShopServiceException ShopDoesNotExist(Shop shop)
    {
        return new ShopServiceException($"Shop {shop.Name} ({shop.ShopAddress}) doesn't exist");
    }
}