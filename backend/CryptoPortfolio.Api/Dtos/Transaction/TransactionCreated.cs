using CryptoPortfolio.Api.Models;

namespace CryptoPortfolio.Api.Dtos.Transaction
{
    public class TransactionCreated
    {
        public int Id { get; set; }
        public CryptoTransaction Info { get; set; }
    }
}
