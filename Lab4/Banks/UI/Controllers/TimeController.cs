using Spectre.Console;

namespace Banks.UI.Controllers;

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
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(">> Success");
        Console.ResetColor();
    }

    public void DateNow()
    {
        Console.WriteLine(_data.CentralBank.RewindClock.CurrentTime());
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(">> Success");
        Console.ResetColor();
    }
}