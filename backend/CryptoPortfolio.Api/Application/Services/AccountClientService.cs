using CryptoPortfolio.Api.Application.Dtos.Client;
using CryptoPortfolio.Api.Common;
using CryptoPortfolio.Api.Domain.Interfaces;
using CryptoPortfolio.Api.Domain.Models;
using CryptoPortfolio.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CryptoPortfolio.Api.Application.Services
{
    public class AccountClientService : IAccountClientService
    {
        private readonly CryptoContext _Context;
        public AccountClientService(CryptoContext context) 
        {
            _Context = context; 
        }

        public async Task<Result<ClientCreated>> CreateClient(ClientCreateDto Dto)
        {
            if (string.IsNullOrWhiteSpace(Dto.Name))
                return Result<ClientCreated>.Error("el nombre es requerido", 400);
            if (string.IsNullOrWhiteSpace(Dto.Email) || !Dto.Email.Contains("@"))
                return Result<ClientCreated>.Error("se requiere un email valido", 400);

            var client = new Client
            {
                Name = Dto.Name.Trim(),
                Email = Dto.Email.Trim()
            };
            _Context.Clients.Add(client);

            await _Context.SaveChangesAsync();

            return Result<ClientCreated>.Exito(new ClientCreated { Id  = client.Id , Name = client.Name});
        }

        public async Task<Result<object>> DeleteClient(int Id)
        {
            var client = await _Context.Clients.FindAsync(Id);
            if (client is null) return Result<object>.Error("cliente no encontrado", 404);

            _Context.Clients.Remove(client);
            await _Context.SaveChangesAsync();
            return Result<object>.Exito(null);
        }

        public async Task<Result<Client>> GetClientById(int Id)
        {
            var client = await _Context.Clients.FindAsync(Id);

            if (client == null) 
            {
                return Result<Client>.Error("cliente no encontrado", 404);
            }

            return Result<Client>.Exito(client);
        }

        public async Task<Result<List<Client>>> GetClients()
        {
            var clients = await _Context.Clients
                .OrderBy(c => c.Name).ToListAsync();

            return Result<List<Client>>.Exito(clients);
        }

        public async Task<Result<object>> UpdateClient(int id ,ClientUpdateDto Dto)
        {
            var client = await _Context.Clients.FindAsync(id);
            if (client is null) return Result<object>.Error("cliente no encontrado", 404);

            if (!string.IsNullOrWhiteSpace(Dto.Name))
                client.Name = Dto.Name.Trim();
            if (!string.IsNullOrWhiteSpace(Dto.Email))
                client.Email = Dto.Email.Trim();

            await _Context.SaveChangesAsync();

            return Result<object>.Exito(null);
        }
    }
}
