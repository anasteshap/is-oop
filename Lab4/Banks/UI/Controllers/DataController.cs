using Banks.Interfaces;

namespace Banks.UI.Controllers;

public class DataController
{
    public DataController(ICentralBank centralBank)
    {
        CentralBank = centralBank;
    }

    public IClient? CurrentClient { get; private set; }
    public ICentralBank CentralBank { get; }

    public void ChangeCurrentClient(IClient client)
    {
        ArgumentNullException.ThrowIfNull(nameof(client));
        CurrentClient = client;
    }
}