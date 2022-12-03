using Banks.Accounts.AccountConfigurations;
using Banks.DateTimeProvider;
using Banks.Entities;
using Banks.Transaction;

namespace Banks.Interfaces;

public interface ICentralBank
{
    RewindClock RewindClock { get; }
    IClient RegisterClient(string name, string surname, string? address = null, long? passport = null);
    Bank? FindBankByName(string name);
    Bank GetBankByName(string name);
    IReadOnlyCollection<Bank> GetAllBanks();
    void CreateBank(string name, decimal debitPercent, List<DepositPercent> depositPercents, decimal creditCommission, decimal creditLimit, decimal limitForDubiousClient, TimeSpan endOfPeriod);
    BankTransaction ReplenishAccount(Guid bankId, Guid accountId, decimal amount); // убрать
    BankTransaction WithdrawMoney(Guid bankId, Guid accountId, decimal amount); // убрать
    BankTransaction TransferMoney(Guid bankId1, Guid accountId1, Guid bankId2, Guid accountId2, decimal amount);
    void CancelTransaction(Guid bankId, Guid accountId, Guid transactionId);
}