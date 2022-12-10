namespace Banks.Observer;

public interface IObserver<T>
{
    void Update(T data);
}