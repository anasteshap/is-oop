using Banks.Accounts;
using Banks.Accounts.AccountConfigurations;
using Banks.Entities;
using Banks.Models;
using Banks.Transaction;

namespace Banks.Interfaces;

public interface ICentralBank
{
    IClient RegisterClient(string name, string surname, string? address = null, long? passport = null);
    void AddClientInfo(IClient client, string? address = null, long passport = default);
    Bank? FindBankByName(string name);
    Bank GetBankByName(string name);
    BaseAccount? FindAccountById(string bankName, Guid accountId);
    IReadOnlyCollection<Bank> GetAllBanks();
    BankConfiguration CreateConfiguration(double debitPercent, Dictionary<Range, Percent> depositPercents, double creditCommission, decimal creditLimit, decimal limitForDubiousClient, uint depositPeriodInDays);
    Bank CreateBank(string name, BankConfiguration bankConfiguration);
    BaseAccount CreateCreditAccount(Bank bank, IClient client);
    BaseAccount CreateDebitAccount(Bank bank, IClient client);
    BaseAccount CreateDepositAccount(Bank bank, IClient client, uint? depositPeriodInDays = null);
    BankTransaction ReplenishAccount(Guid bankId, Guid accountId, decimal amount);
    BankTransaction WithdrawMoney(Guid bankId, Guid accountId, decimal amount);
    BankTransaction TransferMoney(Guid bankId1, Guid accountId1, Guid bankId2, Guid accountId2, decimal amount);
    void CancelTransaction(Guid bankId, Guid accountId, Guid transactionId);
    void ChangeDebitPercent(Guid bankId, double percent);
    void ChangeDepositPercent(Guid bankId, double percent);
    void ChangeCreditCommission(Guid bankId, double commissionPercent);
    void ChangeCreditLimit(Guid bankId, decimal creditLimit);
    void ChangeLimitForDubiousClient(Guid bankId, decimal limitForDubiousClient);
}