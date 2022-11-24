using Banks.Interfaces;

namespace Banks.Observer;

public interface IObservable
{
    void Subscribe(IObserver observer);
    void Unsubscribe(IObserver observer);
    void Notify();
}