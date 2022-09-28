using Shops.Entities;
namespace Shops.Services;

public interface IShopService
{
    void CheckoutNewProductInShop(Product product);
    Shop AddShop(string name, string address);
    void RemoveShop(Shop shop);
    Shop? FindShop(Guid id);
    Shop GetShop(Guid id);
    Customer? FindCustomer(Guid id);
    Customer GetCustomer(Guid id);
    public Shop FindShopWithEnoughProductAndMinPrice(string nameProduct, int amount);
    ShopProduct Delivery(Shop shop, ShopProduct shopProduct);
    public void MakeOrder(Customer customer, Shop shop, string nameProduct, int amount);
    public void MakeCheapestOrder(Customer customer, string nameProduct, int amount);
    void ChangeProductPrice(Shop shop, string productName, decimal newPrice);
    Customer AddCustomer(string name, decimal score = 0);
    Customer RemoveCustomer(Customer customer);
}