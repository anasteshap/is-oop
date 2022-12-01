namespace Banks.Interfaces;

public interface IClient
{
    Guid Id { get; }
    string Name { get; }
    string Surname { get; }
    string Address { get; }
    long PassportNumber { get; }
    void SetAddress(string? address);
    void SetPassportNumber(long passport);
    bool IsDubious();
}