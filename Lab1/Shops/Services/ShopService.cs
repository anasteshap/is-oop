using Shops.Entities;
using Shops.Exceptions;

namespace Shops.Services;

public class ShopService : IShopService
{
    private readonly HashSet<Shop> _shops = new ();
    private readonly HashSet<Customer> _customers = new ();
    private readonly HashSet<Product> _products = new ();

    public void CheckoutNewProductInShop(Product product)
    {
        if (_products.Contains(product))
        {
            throw ShopServiceException.ProductAlreadyRegistered(product);
        }

        _products.Add(product);
    }

    public Shop AddShop(string name, string address)
    {
        var newShop = new Shop(name, address);

        if (_shops.Contains(newShop))
        {
            throw ShopServiceException.ShopAlreadyExist(newShop);
        }

        _shops.Add(newShop);
        return newShop;
    }

    public void RemoveShop(Shop shop)
    {
        if (!_shops.Remove(shop))
        {
            throw ShopServiceException.ShopDoesNotExist(shop);
        }
    }

    public Shop? FindShop(Guid id)
    {
        return _shops.FirstOrDefault(x => x.Id == id);
    }

    public Shop GetShop(Guid id)
    {
        return FindShop(id) ?? throw ShopServiceException.ShopDoesNotExist();
    }

    public Customer? FindCustomer(Guid id)
    {
        return _customers.FirstOrDefault(x => x.Id == id);
    }

    public Customer GetCustomer(Guid id)
    {
        return FindCustomer(id) ?? throw CustomerException.CustomerDoesNotExist();
    }

    public Shop FindShopWithEnoughProductAndMinPrice(string nameProduct, int amount)
    {
        var shops = _shops
            .Where(x => (x.FindProduct(nameProduct)?.Amount ?? -1) >= amount)
            .OrderBy(x => x.GetProduct(nameProduct).Amount)
            .ToList();
        if (!shops.Any())
        {
            throw ShopServiceException.ShopDoesNotExist();
        }

        return shops[0];
    }

    public ShopProduct Delivery(Shop shop, ShopProduct shopProduct)
    {
        if (!_products.Contains(shopProduct.Product))
        {
            throw ShopServiceException.ProductDoesNotRegistered(shopProduct.Product);
        }

        shop.AddProduct(shopProduct);
        return shop.GetProduct(shopProduct.Product.Name);
    }

    public void MakeOrder(Customer customer, Shop shop, string nameProduct, int amount)
    {
        GetCustomer(customer.Id); // проверка есть ли такой покупатель
        GetShop(shop.Id); // проверка есть ли такой магазин

        ShopProduct shopProduct = shop.CheckProductAvailability(nameProduct, amount);
        customer.SpendMoney(shopProduct.Price * amount);
        shop.RemoveSomeProduct(nameProduct, amount);
    }

    public void MakeCheapestOrder(Customer customer, string nameProduct, int amount)
    {
        GetCustomer(customer.Id); // проверка есть ли такой покупатель
        Shop shop = FindShopWithEnoughProductAndMinPrice(nameProduct, amount);
        customer.SpendMoney(shop.GetProduct(nameProduct).Price * amount);
        shop.RemoveSomeProduct(nameProduct, amount);
    }

    public void ChangeProductPrice(Shop shop, string productName, decimal newPrice)
    {
        var product = shop.GetProduct(productName);
        product.ChangePrice(newPrice);
    }

    public Customer AddCustomer(string name, decimal score = 0)
    {
        var customer = new Customer(name, score);
        _customers.Add(customer);
        return customer;
    }

    public Customer RemoveCustomer(Customer customer)
    {
        if (!_customers.Remove(customer))
        {
            throw CustomerException.CustomerDoesNotExist(customer);
        }

        return customer;
    }
}