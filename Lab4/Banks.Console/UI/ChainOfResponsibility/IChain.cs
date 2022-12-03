using System.Collections;

namespace Banks.Console.UI.ChainOfResponsibility;

public interface IChain
{
    void Process(IEnumerator enumerator); // List<string> strings
    IChain AddNext(IChain nextChain);
}