using Spectre.Console;

namespace Banks.Console.UI.Controllers;

public class TimeController
{
    private readonly DataController _data;

    public TimeController(DataController data)
    {
        _data = data;
    }

    public void RewindTime()
    {
        int countOfDays = AnsiConsole.Ask<int>("Enter a [green]countOfDays[/] - ", default);
        _data.CentralBank.RewindClock.RewindTime(new TimeSpan(countOfDays, 0, 0, 0));
        System.Console.ForegroundColor = ConsoleColor.Green;
        System.Console.WriteLine(">> Success");
        System.Console.ResetColor();
    }

    public void DateNow()
    {
        System.Console.WriteLine(_data.CentralBank.RewindClock.CurrentTime());
        System.Console.ForegroundColor = ConsoleColor.Green;
        System.Console.WriteLine(">> Success");
        System.Console.ResetColor();
    }
}