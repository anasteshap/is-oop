namespace Banks.UI.ChainOfResponsibility;

public class ComponentChain : IChain
{
    private readonly string _str; // = "user";
    private Action _action;
    private IChain? _next;

    public ComponentChain(Action action, string str)
    {
        ArgumentNullException.ThrowIfNull(nameof(str));
        _action = action;
        _str = str;
    }

    public bool IsThis(string str) => _str.Equals(str);

    public void Process(List<string> strings)
    {
        if (strings.Count != 1 || !_str.Equals(strings[0]))
        {
            // _next?.Process(strings);
            throw new Exception();
        }

        _action.Invoke();
    }

    public IChain AddNext(IChain nextChain)
    {
        _next = nextChain;
        return nextChain;
    }
}