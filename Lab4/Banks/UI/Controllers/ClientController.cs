using Banks.Interfaces;
using Spectre.Console;

namespace Banks.UI.Controllers;

public class ClientController
{
    private readonly ICentralBank _centralBank;
    public ClientController(ICentralBank centralBank)
    {
        _centralBank = centralBank;
    }

    public void Create()
    {
        string name = AnsiConsole.Ask<string>("Enter a [green]name[/] - ");
        string surName = AnsiConsole.Ask<string>("Enter a [green]surName[/] - ");
        string address = AnsiConsole.Ask("Enter an [green]address[/] - ", string.Empty);
        long passport = AnsiConsole.Ask<long>("Enter a [green]passportNumber[/] - ", default);
        IClient client = _centralBank.RegisterClient(name, surName, address, passport);
        DataObjectController.ChangeCurrentClient(client);
        SuccessfulState();
    }

    public void AddCurrentClientInfo()
    {
        if (DataObjectController.CurrentClient is null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("no registered client");
            Console.ResetColor();
            return;
        }

        string address = AnsiConsole.Ask("Enter an [green]address[/] - ", string.Empty);
        long passport = default;
        if (DataObjectController.CurrentClient.PassportNumber == default)
            passport = AnsiConsole.Ask<long>("Enter a [green]passportNumber[/] - ", default);
        _centralBank.AddClientInfo(DataObjectController.CurrentClient, address, passport);
        SuccessfulState();
    }

    public void ShowCurrentClientInfo()
    {
        var currentClient = DataObjectController.CurrentClient;
        if (currentClient is null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("no registered client");
            Console.ResetColor();
            return;
        }

        Console.WriteLine($"\tName: {currentClient.Name}");
        Console.WriteLine($"\tSurname: {currentClient.Surname}");
        Console.WriteLine($"\tAddress: {currentClient.Address}");
        Console.WriteLine(currentClient.PassportNumber == default
            ? "\tPassportNumber: "
            : $"\tPassportNumber: {currentClient.PassportNumber}");
        SuccessfulState();
    }

    private static void SuccessfulState()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(">> Success");
        Console.ResetColor();
    }
}