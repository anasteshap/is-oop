using Banks.DateTimeProvider;
using Banks.Service;
using Banks.UI.ChainOfResponsibility;
using Banks.UI.Controllers;

namespace Banks.UI;

public class AppManager
{
    private readonly DataController _data;
    private readonly ContainerChain _app = new ("app");

    public AppManager()
    {
        _data = new DataController(new CentralBank(new RewindClock()));
    }

    public void Run()
    {
        Init();
        Menu();
        while (true)
        {
            string? console = Console.ReadLine();
            if (console == "--menu")
            {
                Menu();
                continue;
            }

            try
            {
                var enumerator = console?.Split(" ").ToList().GetEnumerator() ?? throw new Exception();
                if (!enumerator.MoveNext())
                    throw new Exception();
                _app?.Process(enumerator);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Try again\n{e.Message}");
                Console.ResetColor();
                Console.WriteLine("\nwrite for help\n\tapp --menu");
            }
        }
    }

    private static void Menu()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n---Commands---");
        Console.ResetColor();

        Console.WriteLine("app bank create");
        Console.WriteLine("app bank showAll");
        Console.WriteLine("app bank changeConfig");
        Console.WriteLine("app client create");
        Console.WriteLine("app client current addInfo");
        Console.WriteLine("app client current showInfo");
        Console.WriteLine("app client current change");
        Console.WriteLine("app client showAll");
        Console.WriteLine("app account create credit");
        Console.WriteLine("app account create debit");
        Console.WriteLine("app account create deposit");
        Console.WriteLine("app account addMoney");
        Console.WriteLine("app account withdrawMoney");
        Console.WriteLine("app account transferMoney");
        Console.WriteLine("app account cancelTransaction");
        Console.WriteLine("app account showAll");
        Console.WriteLine("app account showAllTransactionsInAccount");
        Console.WriteLine("app rewindTime");
        Console.WriteLine("app dateNow");
    }

    private void Init()
    {
        var controller1 = new BankController(_data);
        var controller2 = new ClientController(_data);
        var controller3 = new AccountController(_data);
        var controller4 = new TransactionController(_data);
        var controller5 = new TimeController(_data);

        var bankContainer = new ContainerChain("bank");
        bankContainer
            .AddSubChain(new ComponentChain(controller1.Create, "create"))
            .AddNext(new ComponentChain(controller1.ShowAll, "showAll"))
            .AddNext(new ComponentChain(controller1.ChangeConfig, "changeConfig"));

        var clientCurrentContainer = new ContainerChain("current");
        clientCurrentContainer
            .AddSubChain(new ComponentChain(controller2.AddCurrentClientInfo, "addInfo"))
            .AddNext(new ComponentChain(controller2.ShowCurrentClientInfo, "showInfo"))
            .AddNext(new ComponentChain(controller2.ChangeCurrent, "change"));

        var clientContainer = new ContainerChain("client");
        clientContainer
            .AddSubChain(new ComponentChain(controller2.Create, "create"))
            .AddNext(clientCurrentContainer)
            .AddNext(new ComponentChain(controller2.ShowAll, "showAll"));

        var accountCreateContainer = new ContainerChain("create");
        accountCreateContainer
            .AddSubChain(new ComponentChain(controller3.CreateCredit, "credit"))
            .AddNext(new ComponentChain(controller3.CreateDebit, "debit"))
            .AddNext(new ComponentChain(controller3.CreateDeposit, "deposit"));

        var accountContainer = new ContainerChain("account");
        accountContainer
            .AddSubChain(accountCreateContainer)
            .AddNext(new ComponentChain(controller4.AddMoney, "addMoney"))
            .AddNext(new ComponentChain(controller4.WithdrawMoney, "withdrawMoney"))
            .AddNext(new ComponentChain(controller4.TransferMoney, "transferMoney"))
            .AddNext(new ComponentChain(controller4.CancelTransaction, "cancelTransaction"))
            .AddNext(new ComponentChain(controller3.ShowAll, "showAll"))
            .AddNext(new ComponentChain(controller4.ShowAllTransactionsInAccount, "showAllTransactionsInAccount"));

        _app
            .AddSubChain(bankContainer)
            .AddNext(clientContainer)
            .AddNext(accountContainer)
            .AddNext(new ComponentChain(controller5.RewindTime, "rewindTime"))
            .AddNext(new ComponentChain(controller5.DateNow, "dateNow"));
    }
}