using Microsoft.Extensions.Caching.Memory;
using Serilog;
using TransactionsManagementProject.Contracts.RepositoryContracts;
using TransactionsManagementProject.Contracts.ServiceContracts;
using TransactionsManagementProject.Implementations;
using TransactionsManagementProject.InfrastructureData;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITransactionsManagementService, TransactionsManagementService>();
builder.Services.AddScoped<IRepositoryCache, RepositoryCache>();
builder.Services.AddScoped<IRepositoryProducts, RepositoryProducts>();
builder.Services.AddScoped<IRepositoryTransformations, RepositoryTransformations>();

builder.Services.AddMemoryCache(memoryCacheOptions =>
{
    memoryCacheOptions.ExpirationScanFrequency = TimeSpan.FromMinutes(1);
    MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
    };
});

builder.Logging.ClearProviders();

var logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();

builder.Logging.AddSerilog(logger);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
