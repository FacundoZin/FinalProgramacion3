using CryptoPortfolio.Api.Common;
using CryptoPortfolio.Api.Domain.Interfaces;
using CryptoPortfolio.Api.Infrastructure.CriptoYaService;

namespace CryptoPortfolio.Api.Application.Services
{
    public class ExchangeService : IExchangeService
    {
        private readonly CriptoYaService _priceService;
        private readonly ILogger<ExchangeService> _logger;

        public ExchangeService(CriptoYaService priceService, ILogger<ExchangeService> logger)
        {
            _priceService = priceService;
            _logger = logger;
        }

        public async Task<Result<object>> GetBestExchange(string cryptoCode, string action)
        {
            if (action != "purchase" && action != "sale")
                return Result<object>.Error("El parámetro 'action' debe ser 'purchase' o 'sale'", 400);

            try
            {
                var best = await _priceService.GetBestExchangeAsync(cryptoCode, action);
                return Result<object>.Exito(best);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to obtain best exchange for {Crypto} ({Action})", cryptoCode, action);
                return Result<object>.Error("No se pudo obtener información de los exchanges en este momento. Intenta nuevamente en unos segundos.", 500);
            }
        }
    }
}
