using CryptoPortfolio.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CryptoPortfolio.Api.Controllers
{
    [ApiController]
    [Route("best-exchange")]
    public class ExchangeController : ControllerBase
    {
        private readonly IExchangeService _exchangeService;

        public ExchangeController(IExchangeService exchangeService)
        {
            _exchangeService = exchangeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBestExchange([FromQuery] string cryptoCode, [FromQuery] string action)
        {
            var result = await _exchangeService.GetBestExchange(cryptoCode, action);

            if (!result.Exit) return StatusCode(result.Errorcode, result.Errormessage);

            return Ok(result.Data);
        }
    }
}
