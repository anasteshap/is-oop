using System.Collections;

namespace Banks.UI.ChainOfResponsibility;

public interface IChain
{
    void Process(IEnumerator enumerator); // List<string> strings
    IChain AddNext(IChain nextChain);
}