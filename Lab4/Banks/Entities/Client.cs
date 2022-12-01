using Banks.Interfaces;
using Banks.Observer;

namespace Banks.Entities;

public class Client : IClient, IObserver
{
    internal Client(string name, string surname, string? address, long? passportNumber)
    {
        Name = name;
        Surname = surname;
        Address = address ?? string.Empty;
        PassportNumber = passportNumber ?? default;
    }

    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; }
    public string Surname { get; }
    public string Address { get; private set; }
    public long PassportNumber { get; private set; }

    public void SetAddress(string? address)
    {
        if (string.IsNullOrEmpty(address))
        {
            throw new Exception();
        }

        Address = address;
    }

    public void SetPassportNumber(long passport)
    {
        if (PassportNumber != default || passport == default)
        {
            throw new Exception();
        }

        PassportNumber = passport;
    }

    public bool IsDubious() => string.IsNullOrEmpty(Address) || PassportNumber == default;

    public void Update()
    {
        throw new NotImplementedException();
    }
}