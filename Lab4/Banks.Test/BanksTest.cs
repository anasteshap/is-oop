using Banks.Accounts;
using Banks.Builders;
using Banks.Entities;
using Banks.Interfaces;
using Banks.Models;
using Banks.Service;
using Xunit;
namespace Banks.Test;

public class BanksTest
{
    [Fact]
    public void Test1()
    {
        var cb = new CentralBank();
        IClient client = cb.RegisterClient("1", "2");
        var depositPercents = new Dictionary<Range, Percent>()
        {
            { new Range(0, 50000), new Percent(3) },
            { new Range(50000, 100000), new Percent(3.5) },
            { Range.StartAt(100000), new Percent(4) },
        };
    }
}