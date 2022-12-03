using System.Globalization;
using Banks.Accounts;
using Banks.Accounts.AccountConfigurations;
using Banks.DateTimeProvider;
using Banks.Entities;
using Banks.Interfaces;
using Banks.Models;
using Banks.Service;
using Banks.Transaction;
using Xunit;
namespace Banks.Test;

public class BanksTest
{
    [Fact]
    public void TransactionTest()
    {
        var cb = new CentralBank(new RewindClock());
        Bank bank = cb.CreateBank("tink", 3, new List<DepositPercent>() { new DepositPercent(new Percent(3), 0) }, 100, 2000, 3000, TimeSpan.FromDays(90));
        IClient client = cb.RegisterClient("Anastasiia", "Pinchuk");
        BaseAccount account1 = bank.CreateAccount(TypeOfBankAccount.Debit, client);

        BankTransaction transaction1 = cb.ReplenishAccount(bank.Id, account1.Id, 100000);
        Assert.True(account1.Balance.Equals(100000m));

        cb.CancelTransaction(bank.Id, account1.Id, transaction1.Id);
        Assert.True(transaction1.TransactionState == State.Failed);

        BaseAccount account2 = bank.CreateAccount(TypeOfBankAccount.Debit, client);
        BankTransaction transaction2 = cb.ReplenishAccount(bank.Id, account2.Id, 500);
        Assert.True(account2.Balance.Equals(500m));

        cb.CancelTransaction(bank.Id, account2.Id, transaction2.Id);
        Assert.True(account2.Balance.Equals(decimal.Zero));
    }
}