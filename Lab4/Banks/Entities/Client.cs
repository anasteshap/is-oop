using Banks.Exceptions;
using Banks.Interfaces;

namespace Banks.Entities;

public class Client : IClient
{
    private readonly List<string> _updates = new ();
    public Client(string name, string surname, string? address, long? passportNumber)
    {
        Name = name;
        Surname = surname;
        Address = address;
        PassportNumber = passportNumber;
    }

    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; }
    public string Surname { get; }
    public string? Address { get; private set; }
    public long? PassportNumber { get; private set; }
    public bool IsDubious => string.IsNullOrEmpty(Address) || PassportNumber == default;
    public IReadOnlyCollection<string> GetUpdatesOfAccountsConfiguration() => _updates;

    public void SetAddress(string? address)
    {
        if (string.IsNullOrEmpty(address))
            throw ClientException.InvalidAddress();

        Address = address;
    }

    public void SetPassportNumber(long passport)
    {
        if (PassportNumber != default)
            throw ClientException.InvalidPassport("Passport is already exist");

        if (passport == default)
            throw ClientException.InvalidPassport();

        PassportNumber = passport;
    }

    public void Update(string data)
    {
        ArgumentNullException.ThrowIfNull(nameof(data));
        _updates.Add(data);
    }
}