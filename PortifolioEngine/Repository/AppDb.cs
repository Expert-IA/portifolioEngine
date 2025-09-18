using InvestorTrust.Contracts.Portfolios;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PortifolioEngine.Domain.Entities;
using System.Text.Json;

namespace PortifolioEngine.Infra.Data;

public class AppDb : DbContext
{
    public AppDb(DbContextOptions<AppDb> options) : base(options) { }

    public DbSet<Portfolio> Portfolios => Set<Portfolio>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        var assetsConverter = new ValueConverter<List<AssetDto>, string>(
            v => JsonSerializer.Serialize(v, jsonOptions),
            v => JsonSerializer.Deserialize<List<AssetDto>>(v, jsonOptions) ?? new List<AssetDto>()
        );

        var assetsComparer = new ValueComparer<List<AssetDto>>(
            (a, b) => JsonSerializer.Serialize(a, jsonOptions) == JsonSerializer.Serialize(b, jsonOptions),
            v => JsonSerializer.Serialize(v, jsonOptions).GetHashCode(),
            v => v == null ? new List<AssetDto>() : v.ToList()
        );


        modelBuilder.Entity<Portfolio>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Name).HasMaxLength(160).IsRequired();
            entity.Property(p => p.TotalAmount).HasPrecision(18, 2);
            entity.Property(p => p.UpdatedAt);

            var assetsProp = entity.Property(p => p.Assets);
            assetsProp.HasConversion(assetsConverter);
            assetsProp.Metadata.SetValueComparer(assetsComparer);
            assetsProp.HasColumnType("jsonb"); // PostgreSQL

            entity.HasIndex(p => p.UserId);
        });
    }
}