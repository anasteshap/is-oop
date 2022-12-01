namespace Banks.UI.ChainOfResponsibility;

public interface IChain
{
    bool IsThis(string str);
    void Process(List<string> strings);
    IChain AddNext(IChain nextChain);
}