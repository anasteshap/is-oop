using Shops.Exceptions;
using Shops.Models;

namespace Shops.Entities;

public class Shop : IEquatable<Shop>
{
    private readonly List<ShopProduct> _shopProducts = new ();

    public Shop(string name, ShopAddress shopAddress)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw ShopException.InvalidName();
        }

        Id = Guid.NewGuid();
        Name = name;
        ShopAddress = shopAddress;
    }

    public Guid Id { get; }
    public string Name { get; }
    public ShopAddress ShopAddress { get; }

    public IReadOnlyList<ShopProduct> ShopProducts() => _shopProducts.AsReadOnly();

    public ShopProduct GetProduct(string name)
    {
        return FindProduct(name) ?? throw ShopProductException.ProductDoesNotExist(name);
    }

    public ShopProduct? FindProduct(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw ShopProductException.InvalidName();
        }

        return _shopProducts.FirstOrDefault(x => x.Product.Name.Equals(name));
    }

    public ShopProduct ChangeProductPrice(string name, decimal newPrice)
    {
        var product = GetProduct(name);
        product.ChangePrice(newPrice);
        return product;
    }

    public void AddProduct(ShopProduct shopProduct)
    {
        if (shopProduct is null)
        {
            throw ShopProductException.InvalidProduct();
        }

        ShopProduct? newProduct = FindProduct(shopProduct.Product.Name);
        if (newProduct is null)
        {
            _shopProducts.Add(shopProduct);
        }
        else
        {
            newProduct.IncreaseAmount(shopProduct.Amount);
            newProduct.ChangePrice(shopProduct.Price);
        }
    }

    public void RemoveSomeProduct(string nameProduct, int amount)
    {
        ShopProduct currentShopProduct = GetProduct(nameProduct);
        currentShopProduct.ReduceAmount(amount);
        if (currentShopProduct.Amount == 0)
        {
            _shopProducts.Remove(currentShopProduct);
        }
    }

    public ShopProduct CheckProductAvailability(string nameProduct, int amount)
    {
        ShopProduct currentShopProduct = GetProduct(nameProduct);
        if (currentShopProduct.Amount < amount)
        {
            throw ShopProductException.NotEnoughAmount(currentShopProduct.Product.Name);
        }

        return currentShopProduct;
    }

    public bool Equals(Shop? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _shopProducts.Equals(other._shopProducts) && Id.Equals(other.Id) && Name == other.Name && ShopAddress.Equals(other.ShopAddress);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Shop)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_shopProducts, Id, Name, ShopAddress);
    }
}