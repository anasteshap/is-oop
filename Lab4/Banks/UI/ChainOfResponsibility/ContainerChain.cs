namespace Banks.UI.ChainOfResponsibility;

public class ContainerChain : IChain
{
    private readonly List<IChain> _chains = new ();
    private readonly string _str;
    public ContainerChain(string str)
    {
        ArgumentNullException.ThrowIfNull(nameof(str));
        _str = str;
    }

    public ContainerChain AddSubChain(IChain subChain)
    {
        if (_chains.Contains(subChain))
        {
            throw new Exception();
        }

        _chains.Add(subChain);

        // subChain.AddNext(this);
        return this;
    }

    public bool IsThis(string str) => _str.Equals(str);

    public void Process(List<string> strings)
    {
        if (strings.Count == 0 || !_str.Equals(strings[0]))
        {
            throw new Exception();
        }

        IChain subChain = _chains.FirstOrDefault(x => x.IsThis(strings[1])) ?? throw new Exception("Try again");
        subChain.Process(strings.GetRange(1, strings.Count - 1));
    }

    public IChain AddNext(IChain nextChain)
    {
        throw new NotImplementedException();
    }
}