using System.Collections;

namespace Banks.Console.UI.ChainOfResponsibility;

public class ComponentChain : ChainBase
{
    private readonly Action _action;
    public ComponentChain(Action action, string str)
        : base(str)
    {
        _action = action;
    }

    public override void Process(IEnumerator enumerator)
    {
        if (!IsThis(enumerator.Current))
        {
            Next?.Process(enumerator);
        }
        else
        {
            if (enumerator.MoveNext())
                throw new Exception();
            _action.Invoke();
        }
    }
}