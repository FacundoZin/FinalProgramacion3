
using CryptoPortfolio.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CryptoPortfolio.Api.Data;

public class CryptoContext : DbContext
{
    public CryptoContext(DbContextOptions<CryptoContext> options) : base(options)
    {
    }

    public DbSet<Client> Clients => Set<Client>();
    public DbSet<CryptoTransaction> Transactions => Set<CryptoTransaction>();
}
