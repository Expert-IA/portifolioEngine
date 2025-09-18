using Microsoft.EntityFrameworkCore;
using PortifolioEngine.Domain.Interfaces;
using PortifolioEngine.Infra.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDb>(o =>
    o.UseNpgsql(builder.Configuration.GetConnectionString("Db")));
// ou PostgreSQL
// o.UseNpgsql(builder.Configuration.GetConnectionString("Db"))

builder.Services.AddDbContext<AppDb>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IPortfolioService, PortfolioService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();