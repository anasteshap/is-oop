using Banks.Accounts;
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

    public Bank RegisterBank(string name, double debitPercent, double depositPercent, double creditCommission, decimal creditLimit, decimal limitForDubiousClient)
    {
        if (_banks.Exists(x => x.Name.Equals(name)))
        {
            throw new Exception();
        }

        var bank = new Bank(name, debitPercent, depositPercent, creditCommission, creditLimit, limitForDubiousClient);
        _banks.Add(bank);
        return bank;
    }

    public BaseAccount CreateBankAccount(Bank bank, IClient client, decimal amount, TypeOfBankAccount typeOfBankAccount)
    {
        return bank.CreateAccount(typeOfBankAccount, client, amount);
    }

    public BankTransaction ReplenishAccount(Guid bankId, Guid accountId, decimal amount)
    {
        Bank bank = _banks.SingleOrDefault(x => x.Id.Equals(bankId)) ?? throw new Exception();
        return bank.Income(accountId, amount);
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