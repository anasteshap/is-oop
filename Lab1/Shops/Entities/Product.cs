using Shops.Exceptions;

namespace Shops.Entities;

public class Product
{
    public Product(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw ShopProductException.InvalidName();
        }

        Name = name;
    }

    public string Name { get; }
}