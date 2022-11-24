using Banks.Interfaces;
using Banks.Observer;

namespace Banks.Entities;

public class Client : IClient, IObserver
{
    private readonly Guid _id = Guid.NewGuid();
    private string? _address;
    private long? _passportNumber;

    internal Client(string name, string surname, string? address, long? passportNumber)
    {
        Name = name;
        Surname = surname;
        _address = address;
        _passportNumber = passportNumber;
    }

    public string Name { get; }
    public string Surname { get; }

    public string Address
    {
        get => _address ?? throw new ArgumentNullException();
        set => _address = value;
    }

    public long PassportNumber
    {
        get => _passportNumber ?? throw new ArgumentNullException();
        set
        {
            if (_passportNumber != null)
            {
                throw new ArgumentNullException();
            }

            _passportNumber = value;
        }
    }

    public bool IsDubious() => _address is null || _passportNumber is null;

    public void Update()
    {
        throw new NotImplementedException();
    }
}