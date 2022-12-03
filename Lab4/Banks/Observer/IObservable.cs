using Banks.Accounts;
using Banks.Interfaces;

namespace Banks.Observer;

public interface IObservable<TSelectType, TData>
{
    void Subscribe(Observer.IObserver<TData> observer);
    void Unsubscribe(Observer.IObserver<TData> observer);
    void Notify(TSelectType selectType, TData data);
}