namespace CryptoPortfolio.Api.Application.Dtos.Transaction
{
    public class TransactionCreateDto
    {
       public string CryptoCode { get; set; }
       public string Action { get; set; }
       public int ClientId { get; set; }
       public decimal CryptoAmount { get; set; }    
       public DateTime DateTime { get; set; }
    }
}
