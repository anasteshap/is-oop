using Banks.Interfaces;

namespace Banks.UI.Controllers;

public class DataObjectController
{
    public DataObjectController(ICentralBank centralBank)
    {
        CentralBank = centralBank;
    }

    public static IClient? CurrentClient { get; private set; }
    public ICentralBank CentralBank { get; }

    public static void ChangeCurrentClient(IClient client)
    {
        ArgumentNullException.ThrowIfNull(nameof(client));
        CurrentClient = client;
    }
}