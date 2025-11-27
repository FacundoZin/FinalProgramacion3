using CryptoPortfolio.Api.Application.Dtos.Transaction;
using CryptoPortfolio.Api.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CryptoPortfolio.Api.Adapters.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;
        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction (TransactionCreateDto dto)
        {
            var result = await _transactionService.CreateTransaction(dto);

            if(!result.Exit) return StatusCode(result.Errorcode, result.Errormessage);

            return Ok(result.Data);
        }

        [HttpGet("client/{idClient:int}")]
        public async Task<IActionResult> GetTransactions(int idClient)
        {
            var result = await _transactionService.GetUserTransactions(idClient);

            if (!result.Exit) return StatusCode(result.Errorcode, result.Errormessage);

            return Ok(result.Data);
        }

        [HttpGet("{idTransaction:int}")]
        public async Task<IActionResult> GetTransactionById(int idTransaction)
        {
            var result = await _transactionService.GetUserTransactions(idTransaction);

            if (!result.Exit) return StatusCode(result.Errorcode, result.Errormessage);

            return Ok(result.Data);
        }

        [HttpPatch("{idTransaction:int}")]
        public async Task<IActionResult> UpdateTransaction(int idTransaction, TransactionUpdateDto dto)
        {
            var result = await _transactionService.UpdateTransactionById(idTransaction, dto);

            if (!result.Exit) return StatusCode(result.Errorcode, result.Errormessage);

            return Ok(result.Data);
        }

        [HttpDelete("{idTransaction:int}")]
        public async Task<IActionResult> DeleteTransaction(int idTransaction)
        {
            var result = await _transactionService.DeleteTransactionById(idTransaction);

            if (!result.Exit) return StatusCode(result.Errorcode, result.Errormessage);

            return Ok(result.Data);
        }
    }
}
