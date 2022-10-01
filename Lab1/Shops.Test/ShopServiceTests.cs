using Shops.Entities;
using Shops.Exceptions;
using Shops.Models;

// using Shops.Exceptions;
using Shops.Services;
using Xunit;
namespace Shops.Test;

public class ShopServiceTests
{
    [Fact]
    public void AddCustomer_BuyProducts()
    {
        var shopService = new ShopService();
        var shop = shopService.AddShop("Food", new ShopAddress("SPb", "Liberty", 65));
        var customer = shopService.AddCustomer("Anastasiia", 1000);

        var product1 = new Product("Apple");
        var product2 = new Product("Cherry");
        shopService.CheckoutNewProduct(product1);
        shopService.CheckoutNewProduct(product2);
        var shopProduct1 = shopService.Delivery(shop, new ShopProduct(product1, 5, 10));
        var shopProduct2 = shopService.Delivery(shop, new ShopProduct(product2, 5, 30));

        Assert.Contains(shopProduct1, shop.ShopProducts());
        Assert.Contains(shopProduct2, shop.ShopProducts());

        shopService.MakeOrder(customer, shop, "Apple", 3);
        shopService.MakeOrder(customer, shop, "Cherry", 5);

        Assert.DoesNotContain(shopProduct2, shop.ShopProducts());
        Assert.True(shopProduct1.Amount == 2);
        Assert.True(customer.Score == 820); // 3 * 10 + 5 * 30
    }

    [Fact]
    public void AddProductInShop_ChangePrice()
    {
        var shopService = new ShopService();
        var shop = shopService.AddShop("Food", new ShopAddress("SPb", "Liberty", 65));

        var product = new Product("Apple");
        shopService.CheckoutNewProduct(product);
        var shopProduct = shopService.Delivery(shop, new ShopProduct(product, 5, 10));

        shop.ChangeProductPrice(product.Name, 15);

        Assert.Contains(shopProduct, shop.ShopProducts());
        Assert.True(shopProduct.Price == 15);
    }

    [Fact]
    public void BuyProductWithMinPrice()
    {
        var shopService = new ShopService();
        var customer = shopService.AddCustomer("Anastasiia", 1000);
        var shop1 = shopService.AddShop("Food1", new ShopAddress("SPb", "Liberty", 65));
        var shop2 = shopService.AddShop("Food2", new ShopAddress("SPb", "Liberty", 65));
        var shop3 = shopService.AddShop("Food3", new ShopAddress("SPb", "Liberty", 65));

        var product = new Product("Apple");
        shopService.CheckoutNewProduct(product);
        shopService.Delivery(shop1, new ShopProduct(product, 5, 20));
        shopService.Delivery(shop2, new ShopProduct(product, 5, 10));
        shopService.Delivery(shop3, new ShopProduct(product, 5, 30));

        var resultShop = shopService.FindShopWithEnoughProductAndMinPrice("Apple", 5);
        shopService.MakeCheapestOrder(customer, "Apple", 5);

        Assert.True(resultShop.Name == "Food2");
        Assert.True(customer.Score == 950); // хуйня
        Assert.Throws<ShopServiceException>(() => shopService.MakeCheapestOrder(customer, "Apple", 10));
    }

    [Fact]
    public void CustomerDoesNotEnoughMoney_ThrowException()
    {
        var shopService = new ShopService();
        var customer = shopService.AddCustomer("Anastasiia", 10);
        var shop = shopService.AddShop("Food", new ShopAddress("SPb", "Liberty", 65));

        var product = new Product("Apple");
        shopService.CheckoutNewProduct(product);
        shopService.Delivery(shop, new ShopProduct(product, 5, 10));

        Assert.Throws<CustomerException>(() => shopService.MakeOrder(customer, shop, "Apple", 5));
    }
}