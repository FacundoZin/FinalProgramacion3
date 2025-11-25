namespace CryptoPortfolio.Api.Dtos.Analysis
{
    public class AnalysisResultDto
    {
        public int ClientId { get; set; }
        public List<AnalysisItemDto> Items { get; set; } = new();
        public decimal TotalMoney { get; set; }
    }
}
