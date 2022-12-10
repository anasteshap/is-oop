using System.Collections;

namespace Banks.Console.UI.ChainOfResponsibility;

public abstract class ChainBase : IChain
{
    private readonly string _str;

    protected ChainBase(string str)
    {
        if (string.IsNullOrEmpty(str))
            throw new ArgumentNullException();
        _str = str;
    }

    protected IChain? Next { get; private set; }
    public abstract void Process(IEnumerator enumerator);

    public IChain AddNext(IChain nextChain)
    {
        Next = nextChain;
        return Next;
    }

    protected bool IsThis(object? str) => _str.Equals(str);
}