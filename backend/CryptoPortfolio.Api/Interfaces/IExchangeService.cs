using CryptoPortfolio.Api.Common;

namespace CryptoPortfolio.Api.Interfaces
{
    public interface IExchangeService
    {
        Task<Result<object>> GetBestExchange(string cryptoCode, string action);
    }
}
