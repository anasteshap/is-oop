using Banks.Entities;
using Banks.Interfaces;
using Banks.Models;
using Spectre.Console;

namespace Banks.UI.Controllers;

public class BankController
{
    private readonly ICentralBank _centralBank;
    public BankController(ICentralBank centralBank)
    {
        _centralBank = centralBank;
    }

    public void Create()
    {
        string bankName = AnsiConsole.Ask<string>("Enter a [green]bank name[/] - ");
        double debitPercent = AnsiConsole.Ask("Enter a [green]bank debitYearPercent[/] - ", 3d);
        double bankBelow50000Percent = AnsiConsole.Ask("Enter a [green]bank bankBelow50000Percent[/] - ", 3d);
        double bankBetween50000And100000Percent = AnsiConsole.Ask("Enter a [green]bank bankBetween50000And100000Percent[/] - ", 4d);
        double bankAbove100000Percent = AnsiConsole.Ask("Enter a [green]bank bankAbove100000Percent[/] - ", 5d);
        double creditCommission = AnsiConsole.Ask("Enter a [green]creditCommission[/] - ", 4d);

        // DateTime bankDepositUnlockDate = AnsiConsole.Ask<DateTime>("Enter a [green]depositUnlockDate[/] - ");
        decimal creditLimit = AnsiConsole.Ask("Enter a [green]creditLimit[/] - ", 5000m);
        decimal limitForDubiousClient = AnsiConsole.Ask("Enter a [green]limitForDubiousClient[/] - ", 3000m);
        uint depositPeriodInDays = AnsiConsole.Ask<uint>("Enter a [green]depositPeriodInDays[/] - ", 91);

        var depositPercents = new Dictionary<Range, Percent>()
        {
            { new Range(0, 50000), new Percent(bankBelow50000Percent) },
            { new Range(50000, 100000), new Percent(bankBetween50000And100000Percent) },
            { Range.StartAt(100000), new Percent(bankAbove100000Percent) },
        };
        _centralBank.CreateBank(bankName, _centralBank.CreateConfiguration(debitPercent, depositPercents, creditCommission, creditLimit, limitForDubiousClient, depositPeriodInDays));
        SuccessfulState();
    }

    public void ShowAll()
    {
        var banks = _centralBank.GetAllBanks().ToList();
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
        Bank? bank = _centralBank.FindBankByName(bankName);
        if (bank is null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("no registered bank");
            Console.ResetColor();
            return;
        }

        char ans;
        ans = AnsiConsole.Ask<char>("Do you want [green]to change debitPercent[/]? (y/n) - ");
        if (ans == 'y')
            _centralBank.ChangeDebitPercent(bank.Id, AnsiConsole.Ask<double>("New [green]debitPercent[/] - "));
        ans = AnsiConsole.Ask<char>("Do you want [green]to change creditCommission[/]? (y/n) - ");
        if (ans == 'y')
            _centralBank.ChangeCreditCommission(bank.Id, AnsiConsole.Ask<double>("New [green]creditCommission[/] - "));
        ans = AnsiConsole.Ask<char>("Do you want [green]to change creditLimit[/]? (y/n) - ");
        if (ans == 'y')
            _centralBank.ChangeCreditLimit(bank.Id, AnsiConsole.Ask<decimal>("New [green]creditLimit[/] - "));
        ans = AnsiConsole.Ask<char>("Do you want [green]to change limitForDubiousClient[/]? (y/n) - ");
        if (ans == 'y')
            _centralBank.ChangeLimitForDubiousClient(bank.Id, AnsiConsole.Ask<decimal>("New [green]limitForDubiousClient[/] - "));
    }

    private static void SuccessfulState()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(">> Success");
        Console.ResetColor();
    }
}