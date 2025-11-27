using System.Net.Http.Json;

namespace CryptoPortfolio.Api.Infrastructure.CriptoYaService;

public class CriptoYaService
{
    private readonly HttpClient _http;

    // Lista fija de exchanges para la mejora "¿Dónde comprar o vender?"
    private static readonly string[] Exchanges = new[]
    {
        "satoshitango",
        "ripio",
        "belo"
    };

    public CriptoYaService(HttpClient http)
    {
        _http = http;
        _http.BaseAddress = new Uri("https://criptoya.com/");
    }

    private record CriptoYaPriceResponse(
        decimal Ask,
        decimal TotalAsk,
        decimal Bid,
        decimal TotalBid,
        long Time
    );

    public async Task<decimal> GetPriceInArsAsync(string cryptoCode, string action)
    {
        // Usamos SatoshiTango como exchange por defecto
        var endpoint = $"api/satoshitango/{cryptoCode.ToLowerInvariant()}/ars";
        var response = await _http.GetFromJsonAsync<CriptoYaPriceResponse>(endpoint);

        if (response is null)
            throw new InvalidOperationException("No pudimos obtener la cotizacion de lasa criptomonedas");

        return action == "purchase" ? response.Ask : response.Bid;
    }

    public async Task<BestExchangeResult> GetBestExchangeAsync(string cryptoCode, string action)
    {

        var tasks = Exchanges.Select(async exchange =>
        {
            try
            {
                var endpoint = $"api/{exchange}/{cryptoCode.ToLowerInvariant()}/ars";
                var res = await _http.GetFromJsonAsync<CriptoYaPriceResponse>(endpoint);

                if (res is null)
                    return (exchange: (string?)null, price: (decimal?)null);

                var price = action == "purchase" ? res.TotalAsk : res.TotalBid;

                return (exchange, price);
            }
            catch
            {
                return (exchange: (string?)null, price: (decimal?)null);
            }
        });

        var results = await Task.WhenAll(tasks);

        var validResults = results.Where(r => r.exchange != null && r.price != null);

        if (!validResults.Any())
            throw new InvalidOperationException("No se pudo obtener información de ningún exchange");

        var best = action == "purchase"
            ? validResults.OrderBy(ex => ex.price).First()
            : validResults.OrderByDescending(ex => ex.price).First();

        return new BestExchangeResult
        {
            CryptoCode = cryptoCode.ToLowerInvariant(),
            Action = action,
            Exchange = best.exchange!,
            Price = best.price!.Value
        };
    }
}

public class BestExchangeResult
{
    public string CryptoCode { get; set; } = "";
    public string Action { get; set; } = "";
    public string Exchange { get; set; } = "";
    public decimal Price { get; set; }
}
