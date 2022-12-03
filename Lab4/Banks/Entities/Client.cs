using Banks.Interfaces;
using Banks.Observer;

namespace Banks.Entities;

public class Client : IClient
{
    private List<string> _updates = new (); // GetMessages() = _updates;
    internal Client(string name, string surname, string? address, long? passportNumber)
    {
        Name = name;
        Surname = surname;
        Address = address;
        PassportNumber = passportNumber;
    }

    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; }
    public string Surname { get; }
    public string? Address { get; private set; } = null;
    public long? PassportNumber { get; private set; } = null;
    public bool IsDubious => string.IsNullOrEmpty(Address) || PassportNumber == default;

    public void SetAddress(string? address)
    {
        if (string.IsNullOrEmpty(address))
            throw new Exception("Invalid address");

        Address = address;
    }

    public void SetPassportNumber(long passport)
    {
        if (PassportNumber != default)
            throw new Exception("Passport is already exist");

        if (passport == default)
            throw new Exception("Invalid passport");

        PassportNumber = passport;
    }

    public void Update(string data)
    {
        ArgumentNullException.ThrowIfNull(nameof(data));
        _updates.Add(data);
    }
}