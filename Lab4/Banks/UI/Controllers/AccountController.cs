using Banks.Entities;
using Banks.Interfaces;
using Spectre.Console;

namespace Banks.UI.Controllers;

public class AccountController
{
    private readonly ICentralBank _centralBank;
    public AccountController(ICentralBank centralBank)
    {
        _centralBank = centralBank;
    }

    public void ShowAll()
    {
        var banks = _centralBank.GetAllBanks();
        if (banks.Count == 0)
        {
            Console.WriteLine("no registered banks and accounts");
        }

        foreach (Bank bank in banks)
        {
            if (bank.GetAccounts.Count == 0)
            {
                Console.WriteLine($"----------Bank name: {bank.Name} :((((");
                continue;
            }

            Console.WriteLine($"----------Bank name: {bank.Name}");
            Console.WriteLine($"----------Bank Id: {bank.Id}");
            Console.WriteLine("Accounts:");
            bank.GetAccounts.ToList().ForEach(x => Console.WriteLine(
                $"Account Id: {x.Id}" +
                $"\nClient Id: {x.Client.Id}" +
                $"\nBalance: {x.Amount}" +
                $"\nType: {x.Type}\n"));
            Console.WriteLine('\n');
        }
    }

    public void CreateCredit()
    {
        if (DataObjectController.CurrentClient is null)
        {
            Console.WriteLine("no registered client, create client");
            return;
        }

        _centralBank.CreateCreditAccount(GetBank(), DataObjectController.CurrentClient);
        SuccessfulState();
    }

    public void CreateDebit()
    {
        if (DataObjectController.CurrentClient is null)
        {
            Console.WriteLine("no registered client, create client");
            return;
        }

        _centralBank.CreateDebitAccount(GetBank(), DataObjectController.CurrentClient);
        SuccessfulState();
    }

    public void CreateDeposit()
    {
        if (DataObjectController.CurrentClient is null)
        {
            Console.WriteLine("no registered client, create client");
            return;
        }

        Bank bank = GetBank();
        char ans = AnsiConsole.Ask<char>("Add a [green]depositPeriodsInDays[/]? (y/n) - ");
        uint? depositPeriodsInDays = null;
        if (ans.Equals('y'))
        {
            depositPeriodsInDays = AnsiConsole.Ask<uint>("Enter a [green]depositPeriodsInDays[/] - ");
        }

        _centralBank.CreateDepositAccount(bank, DataObjectController.CurrentClient, depositPeriodsInDays);
        SuccessfulState();
    }

    private static void SuccessfulState()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(">> Success");
        Console.ResetColor();
    }

    private Bank GetBank()
    {
        string bankName = AnsiConsole.Ask<string>("Enter a [green]bank name[/] - ");
        return _centralBank.GetBankByName(bankName);
    }
}