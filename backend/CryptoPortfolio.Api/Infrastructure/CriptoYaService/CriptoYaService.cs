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
            throw new InvalidOperationException("No response from CriptoYa");

        return action == "purchase" ? response.Ask : response.Bid;
    }

    public async Task<BestExchangeResult> GetBestExchangeAsync(string cryptoCode, string action)
    {
        decimal? bestPrice = null;
        string? bestExchange = null;

        foreach (var ex in Exchanges)
        {
            try
            {
                var endpoint = $"api/{ex}/{cryptoCode.ToLowerInvariant()}/ars";
                var res = await _http.GetFromJsonAsync<CriptoYaPriceResponse>(endpoint);
                if (res is null) continue;

                var price = action == "purchase" ? res.Ask : res.Bid;

                if (bestPrice is null)
                {
                    bestPrice = price;
                    bestExchange = ex;
                }
                else
                {
                    if (action == "purchase")
                    {
                        // conviene el menor precio de compra
                        if (price < bestPrice)
                        {
                            bestPrice = price;
                            bestExchange = ex;
                        }
                    }
                    else
                    {
                        // conviene el mayor precio de venta
                        if (price > bestPrice)
                        {
                            bestPrice = price;
                            bestExchange = ex;
                        }
                    }
                }
            }
            catch
            {
                // ignoramos errores de un exchange
            }
        }

        if (bestPrice is null || bestExchange is null)
            throw new InvalidOperationException("No se pudo obtener información de ningún exchange");

        return new BestExchangeResult
        {
            CryptoCode = cryptoCode.ToLowerInvariant(),
            Action = action,
            Exchange = bestExchange,
            Price = bestPrice.Value
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
