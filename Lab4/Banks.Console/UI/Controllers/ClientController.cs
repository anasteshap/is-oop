using Banks.Interfaces;
using Spectre.Console;

namespace Banks.Console.UI.Controllers;

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
        string? address = AnsiConsole.Ask<string?>("Enter an [green]address[/] - ", null);
        long? passport = AnsiConsole.Ask<long?>("Enter a [green]passportNumber[/] - ", null);
        IClient client = _data.CentralBank.RegisterClient(name, surName, address, passport);
        _data.ChangeCurrentClient(client);
        SuccessfulState();
    }

    public void AddCurrentClientInfo()
    {
        if (_data.CurrentClient is null)
        {
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine("no registered client");
            System.Console.ResetColor();
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
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine("no registered client");
            System.Console.ResetColor();
            return;
        }

        System.Console.WriteLine($"\tName: {_data.CurrentClient.Name}");
        System.Console.WriteLine($"\tSurname: {_data.CurrentClient.Surname}");
        System.Console.WriteLine($"\tAddress: {_data.CurrentClient.Address}");
        System.Console.WriteLine(_data.CurrentClient.PassportNumber == default
            ? "\tPassportNumber: "
            : $"\tPassportNumber: {_data.CurrentClient.PassportNumber}");
        SuccessfulState();
    }

    public void ChangeCurrent()
    {
        Guid id = AnsiConsole.Ask<Guid>("Enter a [green]new clientId[/] - ");
        _data.ChangeCurrentClient(_data.CentralBank.GetClientById(id));
    }

    public void ShowAll()
    {
        Guid id = AnsiConsole.Ask<Guid>("Enter a [green]new clientId[/] - ");
        _data.ChangeCurrentClient(_data.CentralBank.GetClientById(id));
        var clients = _data.CentralBank.GetAlClients();
        if (clients.Count == 0)
        {
            System.Console.WriteLine("no registered clients");
        }

        foreach (IClient client in clients)
        {
            System.Console.WriteLine("---------------------");
            System.Console.WriteLine($"Name: {client.Name}");
            System.Console.WriteLine($"Surname: {client.Surname}");
            System.Console.WriteLine($"Id: {client.Id}");
        }
    }

    private static void SuccessfulState()
    {
        System.Console.ForegroundColor = ConsoleColor.Green;
        System.Console.WriteLine(">> Success");
        System.Console.ResetColor();
    }
}