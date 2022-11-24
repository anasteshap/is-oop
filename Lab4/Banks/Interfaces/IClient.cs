namespace Banks.Interfaces;

public interface IClient
{
    string Name { get; }
    string Surname { get; }
    string Address { get; }
    long PassportNumber { get; }
    bool IsDubious();
}