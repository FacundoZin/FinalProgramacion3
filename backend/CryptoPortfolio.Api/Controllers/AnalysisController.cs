using CryptoPortfolio.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CryptoPortfolio.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AnalysisController : ControllerBase
    {
        private readonly IAnalysisService _analysisService;

        public AnalysisController(IAnalysisService analysisService)
        {
            _analysisService = analysisService;
        }

        [HttpGet("{clientId:int}")]
        public async Task<IActionResult> GetAnalysis(int clientId)
        {
            var result = await _analysisService.GetAnalysis(clientId);

            if (!result.Exit) return StatusCode(result.Errorcode, result.Errormessage);

            return Ok(result.Data);
        }
    }
}
