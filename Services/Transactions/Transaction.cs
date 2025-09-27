using BankingApplication.Models;

namespace BankingApplication.Services.Transactions
{
    public interface ITransaction
    {
        void Create(Account account, decimal amount, string currency);
    }
}
