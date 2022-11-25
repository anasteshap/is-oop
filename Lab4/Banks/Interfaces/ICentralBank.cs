using Banks.Accounts;
using Banks.Entities;
using Banks.Models;
using Banks.Transaction;

namespace Banks.Interfaces;

public interface ICentralBank
{
    // IClient RegisterClient(string name, string surname, string? address = null, long? passport = null);
    // IClient RegisterClient(IClient client);
    Bank RegisterBank(string name, double debitPercent, Dictionary<Range, Percent> depositPercents, double creditCommission, decimal creditLimit, decimal limitForDubiousClient, uint depositPeriodInDays);
    BaseAccount CreateBankAccount(Bank bank, IClient client, decimal amount, TypeOfBankAccount typeOfBankAccount, uint? depositPeriodInDays = null);
    BaseTransaction ReplenishAccount(Guid bankId, Guid accountId, decimal amount);
    BaseTransaction WithdrawMoney(Guid bankId, Guid accountId, decimal amount);
    BaseTransaction TransferMoney(Guid bankId1, Guid accountId1, Guid bankId2, Guid accountId2, decimal amount);
    void CancelTransaction(Guid bankId, Guid accountId, Guid transactionId);
    void ChangeDebitPercent(Guid bankId, double percent);
    void ChangeDepositPercent(Guid bankId, double percent);
    void ChangeCreditCommission(Guid bankId, double commissionPercent);
    void ChangeCreditLimit(Guid bankId, decimal creditLimit);
    void ChangeLimitForDubiousClient(Guid bankId, decimal limitForDubiousClient);
}