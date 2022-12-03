using Banks.Entities;
using Banks.Interfaces;
using Banks.Transaction;
using Spectre.Console;

namespace Banks.UI.Controllers;

public class TransactionController
{
    private readonly DataController _data;

    public TransactionController(DataController data)
    {
        _data = data;
    }

    public void AddMoney()
    {
        string bankName = AnsiConsole.Ask<string>("Enter a [green]bankName[/] - ");
        Bank bank = _data.CentralBank.GetBankByName(bankName);
        Guid accountId = AnsiConsole.Ask<Guid>("Enter an [green]accountId[/] - ");
        bank.GetAccount(accountId);
        decimal sum = AnsiConsole.Ask<decimal>("Enter a [green]sum to add[/] - ");
        BankTransaction transaction = _data.CentralBank.ReplenishAccount(bank.Id, accountId, sum);
        Console.WriteLine(transaction.StatusMessage);
        SuccessfulState();
    }

    public void WithdrawMoney()
    {
        string bankName = AnsiConsole.Ask<string>("Enter a [green]bankName[/] - ");
        Bank bank = _data.CentralBank.GetBankByName(bankName);
        Guid accountId = AnsiConsole.Ask<Guid>("Enter an [green]accountId[/] - ");
        bank.GetAccount(accountId);
        decimal sum = AnsiConsole.Ask<decimal>("Enter a [green]sum to withdraw[/] - ");
        BankTransaction transaction = _data.CentralBank.WithdrawMoney(bank.Id, accountId, sum);
        Console.WriteLine(transaction.StatusMessage);
        SuccessfulState();
    }

    public void TransferMoney()
    {
        string fromBankName = AnsiConsole.Ask<string>("Enter a [green]fromBankName[/] - ");
        Bank fromBank = _data.CentralBank.GetBankByName(fromBankName);
        Guid fromAccountId = AnsiConsole.Ask<Guid>("Enter a [green]fromAccountId[/] - ");
        fromBank.GetAccount(fromAccountId);

        string toBankName = AnsiConsole.Ask<string>("Enter a [green]toBankName[/] - ");
        Bank toBank = _data.CentralBank.GetBankByName(toBankName);
        Guid toAccountId = AnsiConsole.Ask<Guid>("Enter a [green]toAccountId[/] - ");
        toBank.GetAccount(toAccountId);
        decimal sum = AnsiConsole.Ask<decimal>("Enter a [green]sum to transfer[/] - ");
        BankTransaction transaction = _data.CentralBank.TransferMoney(fromBank.Id, fromAccountId, toBank.Id, toAccountId, sum);
        Console.WriteLine(transaction.StatusMessage);
        SuccessfulState();
    }

    public void CancelTransaction()
    {
        string bankName = AnsiConsole.Ask<string>("Enter a [green]bankName[/] - ");
        Bank bank = _data.CentralBank.GetBankByName(bankName);
        Guid accountId = AnsiConsole.Ask<Guid>("Enter an [green]accountId[/] - ");
        bank.GetAccount(accountId);

        Guid transactionId = AnsiConsole.Ask<Guid>("Enter an [green]transactionId[/] - ");
        _data.CentralBank.CancelTransaction(bank.Id, accountId, transactionId);
        BankTransaction transaction = bank
            .GetAccount(accountId)
            .GetAllTransaction
            .Single(x => x.Id.ToString().Equals(transactionId.ToString()));
        Console.WriteLine(transaction.StatusMessage);
        SuccessfulState();
    }

    public void ShowAllTransactionsInAccount()
    {
        string bankName = AnsiConsole.Ask<string>("Enter a [green]bankName[/] - ");
        Bank bank = _data.CentralBank.GetBankByName(bankName);
        Guid accountId = AnsiConsole.Ask<Guid>("Enter an [green]accountId[/] - ");
        bank.GetAccount(accountId);

        var transactions = bank.GetAccount(accountId).GetAllTransaction.ToList();
        Console.WriteLine($"\n>> Bank name: {bank.Name}\n>> BankId: {bank.Id}\n\n>> AccountId: {accountId}\n");
        transactions.ForEach(x => Console.WriteLine($"IdTransaction: {x.Id}"));
        SuccessfulState();
    }

    private static void SuccessfulState()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(">> Success");
        Console.ResetColor();
    }
}