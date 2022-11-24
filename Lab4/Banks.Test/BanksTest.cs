using Banks.Accounts;
using Banks.Entities;
using Banks.Interfaces;
using Banks.Service;
using Xunit;
namespace Banks.Test;

public class BanksTest
{
    [Fact]
    public void Test1()
    {
        /*Client client = new ClientBuilder.ClientBuilder()
            .AddName("1")
            .AddSurname("2")
            .Build();
        Assert.True(client.Name.Equals("1"));
        Assert.True(client.Surname.Equals("2"));
        Assert.Throws<ArgumentNullException>(() => client.Address);*/
        var cb = new CentralBank();
        IClient client = cb.RegisterClient("1", "2");
        Bank bank = cb.RegisterBank("VTB", 3, 3, 3, 100000, 3000, 365);
        BaseAccount creditAccount = cb.CreateBankAccount(bank, client, 100, TypeOfBankAccount.Credit);
        BaseAccount debitAccount = cb.CreateBankAccount(bank, client, 100, TypeOfBankAccount.Debit);
        BaseAccount depositAccount = cb.CreateBankAccount(bank, client, 100, TypeOfBankAccount.Deposit);
    }
}