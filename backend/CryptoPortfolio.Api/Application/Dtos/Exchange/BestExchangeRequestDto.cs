namespace CryptoPortfolio.Api.Application.Dtos.Exchange
{
    public class BestExchangeRequestDto
    {
        public string CryptoCode { get; set; } = "";
        public string Action { get; set; } = "";
    }
}
