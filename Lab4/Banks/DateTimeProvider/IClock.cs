using Banks.Accounts;

namespace Banks.DateTimeProvider;

public interface IClock
{
    DateTime CurrentTime();
    void AddAction(Action action);
}