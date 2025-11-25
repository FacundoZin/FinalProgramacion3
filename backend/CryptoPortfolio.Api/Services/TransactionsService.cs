using CryptoPortfolio.Api.Common;
using CryptoPortfolio.Api.Data;
using CryptoPortfolio.Api.Dtos.Transaction;
using CryptoPortfolio.Api.Interfaces;
using CryptoPortfolio.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CryptoPortfolio.Api.Services
{
    public class TransactionsService : ITransactionService
    {
        private readonly CryptoContext _Context;
        private readonly CriptoYaService _criptoYaService;

        public TransactionsService(CryptoContext cryptoContext, CriptoYaService criptoYaService)
        {
            _criptoYaService = criptoYaService;
            _Context = cryptoContext;
        }

        public async Task<Result<TransactionCreated>> CreateTransaction(TransactionCreateDto dto)
        {
            if (dto.CryptoAmount <= 0)
                return Result<TransactionCreated>.Error("la cantidad de criptomonedas a transferir debe ser mayor a  0", 404);

            if (dto.Action != "purchase" && dto.Action != "sale")
                return Result<TransactionCreated>.Error("la transaccion debe ser de compra o de venta", 404);

            var client = await _Context.Clients.FindAsync(dto.ClientId);
            if (client is null)
                return Result<TransactionCreated>.Error("cliente no encontrado", 400);

            if (dto.Action == "sale")
            {
                var balance = await _Context.Transactions
                    .Where(t => t.ClientId == dto.ClientId && t.CryptoCode == dto.CryptoCode)
                    .SumAsync(t => t.Action == "purchase" ? t.CryptoAmount : -t.CryptoAmount);

                if (balance < dto.CryptoAmount)
                    return Result<TransactionCreated>.Error("El cliente no tiene saldo suficiente para esta venta", 404);
            }

            // obtener precio actual de CriptoYa 
            decimal pricePerUnit;
            try
            {
                pricePerUnit = await _criptoYaService.GetPriceInArsAsync(dto.CryptoCode, dto.Action);
            }
            catch (Exception ex)
            {
                return Result<TransactionCreated>.Error("Error al obtener el precio del servicio externo", 500);
            }

            var money = dto.CryptoAmount * pricePerUnit;

            var transaction = new CryptoTransaction
            {
                CryptoCode = dto.CryptoCode.ToLowerInvariant(),
                Action = dto.Action,
                ClientId = dto.ClientId,
                CryptoAmount = dto.CryptoAmount,
                Money = decimal.Round(money, 2),
                DateTime = dto.DateTime
            };

            _Context.Transactions.Add(transaction);
            await _Context.SaveChangesAsync();

            return Result<TransactionCreated>.Exito(new TransactionCreated { Id = transaction.Id, Info = transaction});
        }

        public async Task<Result<object>> DeleteTransactionById(int idTransaction)
        {
            var transaction = await _Context.Transactions.FindAsync(idTransaction);
            if (transaction is null) return Result<object>.Error("transaccion no encontrada", 400);

            _Context.Transactions.Remove(transaction);
            await _Context.SaveChangesAsync();
            return Result<object>.Exito(null);
        }

        public async Task<Result<CryptoTransaction>> GetTransactionById(int idTransaction)
        {
            var transaction = await _Context.Transactions.FindAsync(idTransaction);
            return transaction is null ? Result<CryptoTransaction>.Error("No se ha podido encontrar la transaccion", 400) : 
                Result<CryptoTransaction>.Exito(transaction);
        }

        public async Task<Result<object>> UpdateTransactionById(int idTransaction, TransactionUpdateDto dto)
        {
            var transaction = await _Context.Transactions.FindAsync(idTransaction);
            if (transaction is null) return Result<object>.Error("transaccion no encontrada", 400);

            if (dto.CryptoAmount.HasValue)
                transaction.CryptoAmount = dto.CryptoAmount.Value;
            if (dto.Money.HasValue)
                transaction.Money = dto.Money.Value;
            if (!string.IsNullOrWhiteSpace(dto.Action))
                transaction.Action = dto.Action;
            if (!string.IsNullOrWhiteSpace(dto.CryptoCode))
                transaction.CryptoCode = dto.CryptoCode.ToLowerInvariant();
            if (dto.DateTime.HasValue)
                transaction.DateTime = dto.DateTime.Value;

            await _Context.SaveChangesAsync();
            return Result<object>.Exito(transaction);
        }

        public async Task<Result<List<CryptoTransaction>>> GetUserTransactions(int clientId)
        {
            var query = _Context.Transactions.AsQueryable();
                
            query = query.Where(t => t.ClientId == clientId);

            var list = await query
                .OrderByDescending(t => t.DateTime)
                .ToListAsync();

            return Result<List<CryptoTransaction>>.Exito(list);
        }
    }
}
