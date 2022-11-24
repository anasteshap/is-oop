using Banks.Accounts;
using Banks.Entities;
using Banks.Transaction;

namespace Banks.Interfaces;

public interface ICentralBank
{
    // IClient RegisterClient(string name, string surname, string? address = null, long? passport = null);
    // IClient RegisterClient(IClient client);
    Bank RegisterBank(string name, double debitPercent, double depositPercent, double creditCommission, decimal creditLimit, decimal limitForDubiousClient);
    BaseAccount CreateBankAccount(Bank bank, IClient client, decimal amount, TypeOfBankAccount typeOfBankAccount);
    BankTransaction ReplenishAccount(Guid bankId, Guid accountId, decimal amount);
    void CancelTransaction(Guid bankId, Guid accountId, Guid transactionId);
    void ChangeDebitPercent(Guid bankId, double percent);
    void ChangeDepositPercent(Guid bankId, double percent);
    void ChangeCreditCommission(Guid bankId, double commissionPercent);
    void ChangeCreditLimit(Guid bankId, decimal creditLimit);
    void ChangeLimitForDubiousClient(Guid bankId, decimal limitForDubiousClient);
}