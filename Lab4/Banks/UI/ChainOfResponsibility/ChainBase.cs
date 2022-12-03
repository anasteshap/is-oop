using System.Collections;

namespace Banks.UI.ChainOfResponsibility;

public abstract class ChainBase : IChain
{
    private readonly string _str;

    protected ChainBase(string str)
    {
        ArgumentNullException.ThrowIfNull(nameof(str));
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