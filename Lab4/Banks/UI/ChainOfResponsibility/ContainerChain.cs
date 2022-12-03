using System.Collections;

namespace Banks.UI.ChainOfResponsibility;

public class ContainerChain : ChainBase
{
    private IChain? _headSubChain;
    public ContainerChain(string str)
        : base(str)
    {
    }

    public IChain AddSubChain(IChain subChain)
    {
        if (_headSubChain is null)
            _headSubChain = subChain;
        else
            _headSubChain.AddNext(subChain);
        return subChain;
    }

    public override void Process(IEnumerator enumerator)
    {
        if (!IsThis(enumerator.Current))
        {
            Next?.Process(enumerator);
        }
        else
        {
            if (!enumerator.MoveNext())
                throw new Exception();
            _headSubChain?.Process(enumerator);
        }
    }
}