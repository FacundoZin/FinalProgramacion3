namespace CryptoPortfolio.Api.Domain.Models
{
    public class CryptoTransaction
    {
        public int Id { get; set; }
        public string CryptoCode { get; set; } = "";
        public string Action { get; set; } = ""; // "purchase" o "sale"
        public int ClientId { get; set; }
        public Client? Client { get; set; }
        public decimal CryptoAmount { get; set; }
        public decimal Money { get; set; }
        public DateTime DateTime { get; set; }
    }
}
