using CryptoPortfolio.Api.Application.Dtos.Transaction;
using CryptoPortfolio.Api.Common;
using CryptoPortfolio.Api.Domain.Models;

namespace CryptoPortfolio.Api.Domain.Interfaces
{
    public interface ITransactionService
    {
        Task<Result<TransactionCreated>> CreateTransaction(TransactionCreateDto dto);
        Task<Result<List<CryptoTransaction>>> GetUserTransactions(int clientId);
        Task<Result<CryptoTransaction>> GetTransactionById(int idTransaction);
        Task<Result<object>> UpdateTransactionById(int idTransaction, TransactionUpdateDto dto);
        Task<Result<object>> DeleteTransactionById(int idTransaction);
    }
}
