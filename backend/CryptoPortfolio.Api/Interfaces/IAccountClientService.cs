using CryptoPortfolio.Api.Common;
using CryptoPortfolio.Api.Dtos.Client;
using CryptoPortfolio.Api.Models;

namespace CryptoPortfolio.Api.Interfaces
{
    public interface IAccountClientService
    {
        Task<Result<ClientCreated>> CreateClient(ClientCreateDto Dto);
        Task<Result<List<Client>>> GetClients();
        Task<Result<Client>> GetClientById(int Id);
        Task<Result<object>> UpdateClient(int id , ClientUpdateDto Dto);
        Task<Result<object>> DeleteClient(int Id);
    }
}
