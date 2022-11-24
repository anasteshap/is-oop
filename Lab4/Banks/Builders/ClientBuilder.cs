using Banks.Entities;

namespace Banks.Builders;

public class ClientBuilder
{
    private string? _name;
    private string? _surname;
    private string? _address;
    private long? _passportNumber;

    public ClientBuilder()
    {
        _name = null;
        _surname = null;
    }

    public ClientBuilder AddName(string? name)
    {
        _name = name;
        return this;
    }

    public ClientBuilder AddSurname(string? surname)
    {
        _surname = surname;
        return this;
    }

    public ClientBuilder AddAddress(string? address)
    {
        _address = address;
        return this;
    }

    public ClientBuilder AddPassportNumber(long? passportNumber)
    {
        _passportNumber = passportNumber;
        return this;
    }

    public Client Build()
    {
        return new Client(
            _name ?? throw new ArgumentNullException(),
            _surname ?? throw new ArgumentNullException(),
            _address,
            _passportNumber);
    }
}