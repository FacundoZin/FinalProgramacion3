using CryptoPortfolio.Api.Common;
using CryptoPortfolio.Api.Dtos.Analysis;

namespace CryptoPortfolio.Api.Interfaces
{
    public interface IAnalysisService
    {
        Task<Result<AnalysisResultDto>> GetAnalysis(int clientId);
    }
}
