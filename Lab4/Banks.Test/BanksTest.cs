using Banks.Accounts;
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
            { new Range(100000, 150000), new Percent(4) },
        };
        Bank bank = cb.RegisterBank("VTB", 3, depositPercents, 3, 100000, 3000, 365);
        BaseAccount creditAccount = cb.CreateBankAccount(bank, client, 100, TypeOfBankAccount.Credit);
        BaseAccount debitAccount = cb.CreateBankAccount(bank, client, 100, TypeOfBankAccount.Debit);
        BaseAccount depositAccount = cb.CreateBankAccount(bank, client, 100, TypeOfBankAccount.Deposit);
    }
}