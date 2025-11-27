using CryptoPortfolio.Api.Application.Dtos.Client;
using CryptoPortfolio.Api.Common;
using CryptoPortfolio.Api.Domain.Models;

namespace CryptoPortfolio.Api.Domain.Interfaces
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
