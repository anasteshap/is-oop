using Banks.Console.UI.ChainOfResponsibility;
using Banks.Console.UI.Controllers;
using Banks.DateTimeProvider;
using Banks.Service;

namespace Banks.Console.UI;

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
            string? console = System.Console.ReadLine();
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
                System.Console.ForegroundColor = ConsoleColor.Blue;
                System.Console.WriteLine($"Try again\n{e.Message}");
                System.Console.ResetColor();
                System.Console.WriteLine("\nwrite for help\n\tapp --menu");
            }
        }
    }

    private static void Menu()
    {
        System.Console.ForegroundColor = ConsoleColor.Yellow;
        System.Console.WriteLine("\n---Commands---");
        System.Console.ResetColor();

        System.Console.WriteLine("app bank create");
        System.Console.WriteLine("app bank showAll");
        System.Console.WriteLine("app bank changeConfig");
        System.Console.WriteLine();
        System.Console.WriteLine("app client create");
        System.Console.WriteLine("app client current addInfo");
        System.Console.WriteLine("app client current showInfo");
        System.Console.WriteLine("app client current change");
        System.Console.WriteLine("app client showAll");
        System.Console.WriteLine();
        System.Console.WriteLine("app account create credit");
        System.Console.WriteLine("app account create debit");
        System.Console.WriteLine("app account create deposit");
        System.Console.WriteLine("app account addMoney");
        System.Console.WriteLine("app account withdrawMoney");
        System.Console.WriteLine("app account transferMoney");
        System.Console.WriteLine("app account cancelTransaction");
        System.Console.WriteLine("app account showAll");
        System.Console.WriteLine("app account showAllTransactionsInAccount");
        System.Console.WriteLine("app account checkBalance");
        System.Console.WriteLine();
        System.Console.WriteLine("app rewindTime");
        System.Console.WriteLine("app dateNow");
        System.Console.WriteLine("app exit");
        System.Console.WriteLine("enter --menu for help");
        System.Console.WriteLine();
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
            .AddNext(new ComponentChain(controller4.ShowAllTransactionsInAccount, "showAllTransactionsInAccount"))
            .AddNext(new ComponentChain(controller3.CheckBalance, "checkBalance"));

        _app
            .AddSubChain(bankContainer)
            .AddNext(clientContainer)
            .AddNext(accountContainer)
            .AddNext(new ComponentChain(controller5.RewindTime, "rewindTime"))
            .AddNext(new ComponentChain(controller5.DateNow, "dateNow"))
            .AddNext(new ComponentChain(controller5.Exit, "exit"));
    }
}