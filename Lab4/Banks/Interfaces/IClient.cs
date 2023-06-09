namespace Banks.Interfaces;

public interface IClient : Observer.IObserver<string>
{
    Guid Id { get; }
    string Name { get; }
    string Surname { get; }
    string? Address { get; }
    long? PassportNumber { get; }
    bool IsDubious { get; }
    IReadOnlyCollection<string> GetUpdatesOfAccountsConfiguration();
    void SetAddress(string? address);
    void SetPassportNumber(long passport);
}