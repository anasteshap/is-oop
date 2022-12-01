using Banks.Interfaces;
using Banks.Service;
using Banks.UI.ChainOfResponsibility;
using Banks.UI.Controllers;

namespace Banks.UI;

public class AppManager
{
    private readonly DataObjectController _dataObject;
    private readonly ContainerChain _app = new ("app");

    public AppManager()
    {
        _dataObject = new DataObjectController(new CentralBank());
    }

    public void Run()
    {
        Init();
        Menu();
        while (true)
        {
            string? console = Console.ReadLine();
            if (console == "app --menu")
            {
                Menu();
                continue;
            }

            try
            {
                _app.Process(console?.Split(" ").ToList() ?? throw new Exception());
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
        Console.WriteLine("app account create credit");
        Console.WriteLine("app account create debit");
        Console.WriteLine("app account create deposit");
        Console.WriteLine("app account addMoney");
        Console.WriteLine("app account withdrawMoney");
        Console.WriteLine("app account showAll");
    }

    private void Init()
    {
        var controller1 = new BankController(_dataObject.CentralBank);
        var controller2 = new ClientController(_dataObject.CentralBank);
        var controller3 = new AccountController(_dataObject.CentralBank);
        var controller4 = new TransactionController(_dataObject.CentralBank);

        _app
            .AddSubChain(
                new ContainerChain("bank")
                    .AddSubChain(new ComponentChain(controller1.Create, "create"))
                    .AddSubChain(new ComponentChain(controller1.ShowAll, "showAll"))
                    .AddSubChain(new ComponentChain(controller1.ChangeConfig, "changeConfig")))
            .AddSubChain(
                new ContainerChain("client")
                    .AddSubChain(new ComponentChain(controller2.Create, "create"))
                    .AddSubChain(
                        new ContainerChain("current")
                            .AddSubChain(new ComponentChain(controller2.AddCurrentClientInfo, "addInfo"))
                            .AddSubChain(new ComponentChain(controller2.ShowCurrentClientInfo, "showInfo"))))
            .AddSubChain(
                new ContainerChain("account")
                    .AddSubChain(new ContainerChain("create")
                        .AddSubChain(new ComponentChain(controller3.CreateCredit, "credit"))
                        .AddSubChain(new ComponentChain(controller3.CreateDebit, "debit"))
                        .AddSubChain(new ComponentChain(controller3.CreateDeposit, "deposit")))
                    .AddSubChain(new ComponentChain(controller4.AddMoney, "addMoney"))
                    .AddSubChain(new ComponentChain(controller4.WithdrawMoney, "withdrawMoney"))
                    .AddSubChain(new ComponentChain(controller4.TransferMoney, "transferMoney"))
                    .AddSubChain(new ComponentChain(controller3.ShowAll, "showAll")));
    }
}