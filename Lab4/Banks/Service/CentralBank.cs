using Banks.Accounts;
using Banks.Accounts.Commands;
using Banks.Builders;
using Banks.Entities;
using Banks.Interfaces;
using Banks.Transaction;

namespace Banks.Service;

public class CentralBank : ICentralBank
{
    private readonly List<Bank> _banks = new ();

    public IClient RegisterClient(string name, string surname, string? address = null, long? passport = null)
    {
        return new ClientBuilder().AddName(name).AddSurname(surname).AddAddress(address).AddPassportNumber(passport).Build();
    }

    public IClient RegisterClient(IClient client)
    {
        throw new NotImplementedException();
    }

    public Bank RegisterBank(string name, double debitPercent, double depositPercent, double creditCommission, decimal creditLimit, decimal limitForDubiousClient, uint depositPeriodInDays)
    {
        if (_banks.Exists(x => x.Name.Equals(name)))
        {
            throw new Exception();
        }

        var bank = new Bank(name, debitPercent, depositPercent, creditCommission, creditLimit, limitForDubiousClient, depositPeriodInDays);
        _banks.Add(bank);
        return bank;
    }

    public BaseAccount CreateBankAccount(Bank bank, IClient client, decimal amount, TypeOfBankAccount typeOfBankAccount)
    {
        return bank.CreateAccount(typeOfBankAccount, client, amount);
    }

    public BaseTransaction ReplenishAccount(Guid bankId, Guid accountId, decimal amount)
    {
        Bank bank = _banks.SingleOrDefault(x => x.Id.Equals(bankId)) ?? throw new Exception();
        return bank.Income(accountId, amount);
    }

    public BaseTransaction WithdrawMoney(Guid bankId, Guid accountId, decimal amount)
    {
        Bank bank = _banks.SingleOrDefault(x => x.Id.Equals(bankId)) ?? throw new Exception();
        return bank.Withdraw(accountId, amount);
    }

    public BaseTransaction TransferMoney(Guid bankId1, Guid accountId1, Guid bankId2, Guid accountId2, decimal amount)
    {
        Bank bank1 = _banks.SingleOrDefault(x => x.Id.Equals(bankId1)) ?? throw new Exception();
        Bank bank2 = _banks.SingleOrDefault(x => x.Id.Equals(bankId2)) ?? throw new Exception();
        BaseAccount fromAccount = bank1.GetAccount(accountId1);
        BaseAccount toAccount = bank1.GetAccount(accountId2);

        var transactionFrom = new ChainTransaction(DateTime.Now, new Withdraw(fromAccount, amount));
        var transactionTo = new ChainTransaction(DateTime.Now, new Income(toAccount, amount));
        transactionFrom.SetNext(transactionTo);

        transactionFrom.DoTransaction();
        transactionTo.DoTransaction();
        return transactionTo;
    }

    public void CancelTransaction(Guid bankId, Guid accountId, Guid transactionId)
    {
        Bank bank = _banks.SingleOrDefault(x => x.Id.Equals(bankId)) ?? throw new Exception();
        bank.GetAccount(accountId).GetTransaction(transactionId).Undo();
    }

    public void ChangeDebitPercent(Guid bankId, double percent)
    {
        throw new NotImplementedException();
    }

    public void ChangeDepositPercent(Guid bankId, double percent)
    {
        throw new NotImplementedException();
    }

    public void ChangeCreditCommission(Guid bankId, double commissionPercent)
    {
        throw new NotImplementedException();
    }

    public void ChangeCreditLimit(Guid bankId, decimal creditLimit)
    {
        throw new NotImplementedException();
    }

    public void ChangeLimitForDubiousClient(Guid bankId, decimal limitForDubiousClient)
    {
        throw new NotImplementedException();
    }
}