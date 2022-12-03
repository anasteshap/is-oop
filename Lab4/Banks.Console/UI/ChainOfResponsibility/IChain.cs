using System.Collections;

namespace Banks.Console.UI.ChainOfResponsibility;

public interface IChain
{
    void Process(IEnumerator enumerator);
    IChain AddNext(IChain nextChain);
}