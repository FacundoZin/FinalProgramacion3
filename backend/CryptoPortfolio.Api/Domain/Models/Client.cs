namespace CryptoPortfolio.Api.Domain.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public ICollection<CryptoTransaction> Transactions { get; set; } = new List<CryptoTransaction>();
    }

}
