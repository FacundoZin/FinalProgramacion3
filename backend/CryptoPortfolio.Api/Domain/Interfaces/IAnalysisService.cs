using CryptoPortfolio.Api.Application.Dtos.Analysis;
using CryptoPortfolio.Api.Common;

namespace CryptoPortfolio.Api.Domain.Interfaces
{
    public interface IAnalysisService
    {
        Task<Result<AnalysisResultDto>> GetAnalysis(int clientId);
    }
}
