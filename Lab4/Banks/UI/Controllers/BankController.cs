using Banks.Accounts.AccountConfigurations;
using Banks.Entities;
using Banks.Interfaces;
using Banks.Models;
using Spectre.Console;

namespace Banks.UI.Controllers;

public class BankController
{
    private readonly DataController _data;
    public BankController(DataController data)
    {
        _data = data;
    }

    public void Create()
    {
        string bankName = AnsiConsole.Ask<string>("Enter a [green]bank name[/] - ");
        decimal debitPercent = AnsiConsole.Ask("Enter a [green]bank debitYearPercent[/] - ", 3);
        var depositPercents = new List<DepositPercent>() { };
        Console.WriteLine("Enter a bank depositPercents\n");
        while (true)
        {
            decimal percent = AnsiConsole.Ask<decimal>("Enter a [green]bank depositPercent[/] - ");
            decimal leftBorder = AnsiConsole.Ask<decimal>("Enter a [green]leftBorder for depositPercent[/] - ");
            decimal rightBorder =
                AnsiConsole.Ask("Enter a [green]rightBorder for depositPercent[/] - ", decimal.Zero);
            if (rightBorder == decimal.Zero)
                break;
            depositPercents.Add(new DepositPercent(new Percent(percent), leftBorder, rightBorder));
        }

        decimal creditCommission = AnsiConsole.Ask("Enter a [green]creditCommission[/] - ", 200m);
        int countOfDays = AnsiConsole.Ask("Enter a [green]countOfDays[/] - ", 90);
        decimal creditLimit = AnsiConsole.Ask("Enter a [green]creditLimit[/] - ", 5000m);
        decimal limitForDubiousClient = AnsiConsole.Ask("Enter a [green]limitForDubiousClient[/] - ", 3000m);

        _data.CentralBank.CreateBank(bankName, debitPercent, depositPercents, creditCommission, creditLimit, limitForDubiousClient, new TimeSpan(countOfDays, 0, 0, 0));
        SuccessfulState();
    }

    public void ShowAll()
    {
        var banks = _data.CentralBank.GetAllBanks().ToList();
        if (banks.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("no registered bank");
            Console.ResetColor();
            return;
        }

        banks.ForEach(x => Console.WriteLine($"\nName: {x.Name}\nId: {x.Id}"));
        Console.WriteLine("\n");
    }

    public void ChangeConfig()
    {
        string bankName = AnsiConsole.Ask<string>("[green]Bank name[/] - ");
        Bank? bank = _data.CentralBank.FindBankByName(bankName);
        if (bank is null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("no registered bank");
            Console.ResetColor();
            return;
        }

        char ans = AnsiConsole.Ask<char>("Do you want [green]to change debitPercent[/]? (y/n) - ");
        if (ans == 'y')
            bank.ChangeDebitPercent(AnsiConsole.Ask<decimal>("New [green]debitPercent[/] - "));
        ans = AnsiConsole.Ask<char>("Do you want [green]to change depositPercents[/]? (y/n) - ");
        if (ans == 'y')
        {
            var depositPercents = new List<DepositPercent>() { };
            Console.WriteLine("New depositPercents\n");
            while (true)
            {
                decimal percent = AnsiConsole.Ask<decimal>("Enter a [green]bank depositPercent[/] - ");
                decimal leftBorder = AnsiConsole.Ask<decimal>("Enter a [green]leftBorder for depositPercent[/] - ");
                decimal rightBorder =
                    AnsiConsole.Ask("Enter a [green]rightBorder for depositPercent[/] - ", decimal.Zero);
                if (rightBorder == decimal.Zero)
                    break;
                depositPercents.Add(new DepositPercent(new Percent(percent), leftBorder, rightBorder));
            }

            bank.ChangeDepositPercent(depositPercents);
        }

        ans = AnsiConsole.Ask<char>("Do you want [green]to change creditCommission[/]? (y/n) - ");
        if (ans == 'y')
            bank.ChangeCreditCommission(AnsiConsole.Ask<decimal>("New [green]creditCommission[/] - "));
        ans = AnsiConsole.Ask<char>("Do you want [green]to change creditLimit[/]? (y/n) - ");
        if (ans == 'y')
            bank.ChangeCreditLimit(AnsiConsole.Ask<decimal>("New [green]creditLimit[/] - "));
        ans = AnsiConsole.Ask<char>("Do you want [green]to change limitForDubiousClient[/]? (y/n) - ");
        if (ans == 'y')
            bank.ChangeLimitForDubiousClient(AnsiConsole.Ask<decimal>("New [green]limitForDubiousClient[/] - "));
    }

    private static void SuccessfulState()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(">> Success");
        Console.ResetColor();
    }
}