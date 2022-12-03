using Banks.Accounts;
using Banks.Entities;
using Spectre.Console;

namespace Banks.Console.UI.Controllers;

public class AccountController
{
    private readonly DataController _data;
    public AccountController(DataController data)
    {
        _data = data;
    }

    public void ShowAll()
    {
        var banks = _data.CentralBank.GetAllBanks();
        if (banks.Count == 0)
        {
            System.Console.WriteLine("no registered banks and accounts");
        }

        foreach (Bank bank in banks)
        {
            if (bank.GetAccounts.Count == 0)
            {
                System.Console.WriteLine($"----------Bank name: {bank.Name} :((((");
                continue;
            }

            System.Console.WriteLine($"----------Bank name: {bank.Name}");
            System.Console.WriteLine($"----------Bank Id: {bank.Id}");
            System.Console.WriteLine("Accounts:");
            bank.GetAccounts.ToList().ForEach(x => System.Console.WriteLine(
                $"Account Id: {x.Id}" +
                $"\nClient Id: {x.Client.Id}" +
                $"\nBalance: {x.Balance}" +
                $"\nType: {x.Type}\n"));
        }
    }

    public void CreateCredit()
    {
        if (_data.CurrentClient is null)
        {
            System.Console.WriteLine("no registered client, create client");
            return;
        }

        GetBank().CreateAccount(TypeOfBankAccount.Credit, _data.CurrentClient);
        SuccessfulState();
    }

    public void CreateDebit()
    {
        if (_data.CurrentClient is null)
        {
            System.Console.WriteLine("no registered client, create client");
            return;
        }

        GetBank().CreateAccount(TypeOfBankAccount.Debit, _data.CurrentClient);
        SuccessfulState();
    }

    public void CreateDeposit()
    {
        if (_data.CurrentClient is null)
        {
            System.Console.WriteLine("no registered client, create client");
            return;
        }

        Bank bank = GetBank();
        char ans = AnsiConsole.Ask<char>("Add a [green]depositPeriodsInDays[/]? (y/n) - ");
        TimeSpan? endOfPeriod = null;
        if (ans.Equals('y'))
        {
            endOfPeriod = AnsiConsole.Ask("Enter a [green]timeSpan[/] - ", TimeSpan.FromDays(90));
        }

        bank.CreateAccount(TypeOfBankAccount.Deposit, _data.CurrentClient, endOfPeriod);
        SuccessfulState();
    }

    private static void SuccessfulState()
    {
        System.Console.ForegroundColor = ConsoleColor.Green;
        System.Console.WriteLine(">> Success");
        System.Console.ResetColor();
    }

    private Bank GetBank()
    {
        string bankName = AnsiConsole.Ask<string>("Enter a [green]bank name[/] - ");
        return _data.CentralBank.GetBankByName(bankName);
    }
}