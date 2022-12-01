using Banks.Entities;
using Banks.Interfaces;
using Spectre.Console;

namespace Banks.UI.Controllers;

public class TransactionController
{
    private readonly ICentralBank _centralBank;

    public TransactionController(ICentralBank centralBank)
    {
        _centralBank = centralBank;
    }

    public void AddMoney()
    {
    }

    public void WithdrawMoney()
    {
    }

    public void TransferMoney()
    {
        string fromBankName = AnsiConsole.Ask<string>("Enter a [green]fromBankName[/] - ");
        Bank fromBank = _centralBank.GetBankByName(fromBankName);
        Guid fromAccountId = AnsiConsole.Ask<Guid>("Enter a [green]fromAccountId[/] - ");

        string toBankName = AnsiConsole.Ask<string>("Enter a [green]toBankName[/] - ");
        Bank toBank = _centralBank.GetBankByName(toBankName);
        Guid toAccountId = AnsiConsole.Ask<Guid>("Enter a [green]toAccountId[/] - ");

        decimal sum = AnsiConsole.Ask<decimal>("Enter a [green]sum to transfer[/] - ");
        _centralBank.TransferMoney(fromBank.Id, fromAccountId, toBank.Id, toAccountId, sum);
        SuccessfulState();
    }

    private static void SuccessfulState()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(">> Success");
        Console.ResetColor();
    }
}