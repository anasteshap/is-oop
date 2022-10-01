using Shops.Exceptions;
namespace Shops.Models;

public class ShopAddress
{
    public ShopAddress(string city, string street, int number)
    {
        if (string.IsNullOrEmpty(city) || string.IsNullOrEmpty(street))
        {
            throw ShopException.InvalidAddress();
        }

        if (number <= 0)
        {
            throw ShopException.InvalidAddress();
        }

        Address = city + " " + street + " " + number.ToString();
    }

    public string Address { get; }
}