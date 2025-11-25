using CryptoPortfolio.Api.Common;
using CryptoPortfolio.Api.Data;
using CryptoPortfolio.Api.Dtos.Analysis;
using CryptoPortfolio.Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CryptoPortfolio.Api.Services
{
    public class AnalysisService : IAnalysisService
    {
        private readonly CryptoContext _context;
        private readonly CriptoYaService _priceService;

        public AnalysisService(CryptoContext context, CriptoYaService priceService)
        {
            _context = context;
            _priceService = priceService;
        }

        public async Task<Result<AnalysisResultDto>> GetAnalysis(int clientId)
        {
            var transactions = await _context.Transactions
                .Where(t => t.ClientId == clientId)
                .ToListAsync();

            var holdings = transactions
                .GroupBy(t => t.CryptoCode)
                .Select(g => new
                {
                    CryptoCode = g.Key,
                    Amount = g.Sum(t => t.Action == "purchase" ? t.CryptoAmount : -t.CryptoAmount)
                })
                .Where(x => x.Amount > 0)
                .ToList();

            var result = new List<AnalysisItemDto>();
            decimal total = 0;

            foreach (var h in holdings)
            {
                var price = await _priceService.GetPriceInArsAsync(h.CryptoCode, "sale");
                var money = decimal.Round(h.Amount * price, 2);
                total += money;

                result.Add(new AnalysisItemDto
                {
                    CryptoCode = h.CryptoCode,
                    Amount = h.Amount,
                    Money = money
                });
            }

            var response = new AnalysisResultDto
            {
                ClientId = clientId,
                Items = result,
                TotalMoney = total
            };

            return Result<AnalysisResultDto>.Exito(response);
        }
    }
}
