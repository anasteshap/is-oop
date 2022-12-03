using Banks.Interfaces;
using Spectre.Console;

namespace Banks.UI.Controllers;

public class ClientController
{
    private readonly DataController _data;
    public ClientController(DataController data)
    {
        _data = data;
    }

    public void Create()
    {
        string name = AnsiConsole.Ask<string>("Enter a [green]name[/] - ");
        string surName = AnsiConsole.Ask<string>("Enter a [green]surName[/] - ");
        string address = AnsiConsole.Ask("Enter an [green]address[/] - ", string.Empty);
        long passport = AnsiConsole.Ask<long>("Enter a [green]passportNumber[/] - ", default);
        IClient client = _data.CentralBank.RegisterClient(name, surName, address, passport);
        _data.ChangeCurrentClient(client);
        SuccessfulState();
    }

    public void AddCurrentClientInfo()
    {
        if (_data.CurrentClient is null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("no registered client");
            Console.ResetColor();
            return;
        }

        string address = AnsiConsole.Ask("Enter an [green]address[/] - ", string.Empty);
        long passport = default;
        if (_data.CurrentClient.PassportNumber == default)
            passport = AnsiConsole.Ask<long>("Enter a [green]passportNumber[/] - ", default);
        if (!string.IsNullOrEmpty(address))
            _data.CurrentClient.SetAddress(address);
        if (passport != default)
            _data.CurrentClient.SetPassportNumber(passport);
        SuccessfulState();
    }

    public void ShowCurrentClientInfo()
    {
        if (_data.CurrentClient is null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("no registered client");
            Console.ResetColor();
            return;
        }

        Console.WriteLine($"\tName: {_data.CurrentClient.Name}");
        Console.WriteLine($"\tSurname: {_data.CurrentClient.Surname}");
        Console.WriteLine($"\tAddress: {_data.CurrentClient.Address}");
        Console.WriteLine(_data.CurrentClient.PassportNumber == default
            ? "\tPassportNumber: "
            : $"\tPassportNumber: {_data.CurrentClient.PassportNumber}");
        SuccessfulState();
    }

    private static void SuccessfulState()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(">> Success");
        Console.ResetColor();
    }
}