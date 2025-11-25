using CryptoPortfolio.Api.Dtos.Client;
using CryptoPortfolio.Api.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CryptoPortfolio.Api.Controllers
{
    public class ClientController : Controller
    {
        private readonly IAccountClientService _accountClientService;
        public ClientController(IAccountClientService clientService)
        {
            _accountClientService = clientService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] ClientCreateDto Dto)
        {
            var result = await _accountClientService.CreateClient(Dto);

            if(!result.Exit) return StatusCode(result.Errorcode, result.Errormessage);

            return Created(result.Data.Name, result.Data.Id);
        }

        [HttpGet]
        public async Task<IActionResult> GetClients()
        {
            var result = await _accountClientService.GetClients();

            if (!result.Exit) return StatusCode(result.Errorcode, result.Errormessage);

            return Ok(result.Data);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetClientById(int id)
        {
            var result = await _accountClientService.GetClientById(id);

            if(!result.Exit) return StatusCode(result.Errorcode,result.Errormessage);

            return Ok(result.Data);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateClient (int id, [FromBody] ClientUpdateDto dto)
        {
            var result = await _accountClientService.UpdateClient(id, dto);

            if (!result.Exit) return StatusCode(result.Errorcode, result.Errormessage);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteClient (int id)
        {
            var result = await _accountClientService.DeleteClient(id);

            if (!result.Exit) return StatusCode(result.Errorcode, result.Errormessage);

            return NoContent();
        }

    }
}
