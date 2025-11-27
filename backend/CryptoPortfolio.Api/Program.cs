using Microsoft.EntityFrameworkCore;
using CryptoPortfolio.Api.Application.Services;
using CryptoPortfolio.Api.Infrastructure.CriptoYaService;
using CryptoPortfolio.Api.Domain.Interfaces;
using CryptoPortfolio.Api.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddDbContext<CryptoContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=crypto.db"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

// Register services
builder.Services.AddScoped<CriptoYaService>();
builder.Services.AddScoped<IAccountClientService, AccountClientService>();
builder.Services.AddScoped<ITransactionService, TransactionsService>();
builder.Services.AddScoped<IAnalysisService, AnalysisService>();
builder.Services.AddScoped<IExchangeService, ExchangeService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Ensure DB exists
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CryptoContext>();
    db.Database.EnsureCreated();
}

app.UseRouting();
app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.MapGet("/", () => "Crypto Portfolio API is running");


app.Run();
