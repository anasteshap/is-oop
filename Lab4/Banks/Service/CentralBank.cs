using Banks.Accounts;
using Banks.Accounts.Commands;
using Banks.Builders;
using Banks.Entities;
using Banks.Interfaces;
using Banks.Models;
using Banks.Transaction;

namespace Banks.Service;

public class CentralBank : ICentralBank
{
    private readonly List<Bank> _banks = new ();
    private readonly List<BankTransaction> _transactions = new ();

    public IClient RegisterClient(string name, string surname, string? address = null, long? passport = null)
    {
        return new ClientBuilder().AddName(name).AddSurname(surname).AddAddress(address).AddPassportNumber(passport).Build();
    }

    public IClient RegisterClient(IClient client)
    {
        throw new NotImplementedException();
    }

    public Bank RegisterBank(string name, double debitPercent, Dictionary<Range, Percent> depositPercents, double creditCommission, decimal creditLimit, decimal limitForDubiousClient, uint depositPeriodInDays)
    {
        if (_banks.Exists(x => x.Name.Equals(name)))
        {
            throw new Exception();
        }

        var bank = new Bank(name, debitPercent, depositPercents, creditCommission, creditLimit, limitForDubiousClient, depositPeriodInDays);
        _banks.Add(bank);
        return bank;
    }

    // или так (1 метод)
    public BaseAccount CreateBankAccount(Bank bank, IClient client, decimal amount, TypeOfBankAccount typeOfBankAccount, uint? depositPeriodInDays = null)
    {
        return bank.CreateAccount(typeOfBankAccount, client, amount, depositPeriodInDays);
    }

    // или так (3 метода)
    public BaseAccount CreateCreditAccount(Bank bank, IClient client)
    {
        return bank.CreateAccount(TypeOfBankAccount.Credit, client, 0); // разобраться с суммой
    }

    public BaseAccount CreateDebitAccount(Bank bank, IClient client)
    {
        return bank.CreateAccount(TypeOfBankAccount.Debit, client, 0); // разобраться с суммой
    }

    public BaseAccount CreateDepositAccount(Bank bank, IClient client, uint? depositPeriodInDays = null)
    {
        return bank.CreateAccount(TypeOfBankAccount.Deposit, client, 0, depositPeriodInDays); // разобраться с суммой
    }

    public BankTransaction ReplenishAccount(Guid bankId, Guid accountId, decimal amount)
    {
        Bank bank = _banks.SingleOrDefault(x => x.Id.Equals(bankId)) ?? throw new Exception();
        return bank.Income(accountId, amount);
    }

    public BankTransaction WithdrawMoney(Guid bankId, Guid accountId, decimal amount)
    {
        Bank bank = _banks.SingleOrDefault(x => x.Id.Equals(bankId)) ?? throw new Exception();
        return bank.Withdraw(accountId, amount);
    }

    public BankTransaction TransferMoney(Guid bankId1, Guid accountId1, Guid bankId2, Guid accountId2, decimal amount)
    {
        Bank bank1 = _banks.SingleOrDefault(x => x.Id.Equals(bankId1)) ?? throw new Exception();
        Bank bank2 = _banks.SingleOrDefault(x => x.Id.Equals(bankId2)) ?? throw new Exception();
        BaseAccount fromAccount = bank1.GetAccount(accountId1);
        BaseAccount toAccount = bank2.GetAccount(accountId2);

        var transactionFrom = new ChainTransaction(new Withdraw(fromAccount, amount));
        var transactionTo = new ChainTransaction(new Income(toAccount, amount));
        transactionFrom.SetNext(transactionTo);

        transactionFrom.DoTransaction();
        fromAccount.SaveChanges(transactionFrom);

        transactionTo.DoTransaction();
        toAccount.SaveChanges(transactionTo);

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