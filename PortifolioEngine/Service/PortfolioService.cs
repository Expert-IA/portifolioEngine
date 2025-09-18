// Application/Services/PortfolioService.cs

using InvestorTrust.Contracts.Portfolios;
using Microsoft.EntityFrameworkCore;
using PortifolioEngine.Domain.Entities;
using PortifolioEngine.Domain.Interfaces;
using PortifolioEngine.Infra.Data;

public class PortfolioService : IPortfolioService
{
    private readonly AppDb _db;
    public PortfolioService(AppDb db) => _db = db;

    public async Task<PortfolioResponseDto> CreateAsync(PortfolioCreateDto dto)
    {
        var entity = new Portfolio
        {
            Id = Guid.NewGuid(),
            UserId = dto.UserId,
            Name = dto.Name,
            TotalAmount = dto.TotalAmount,
            Assets = dto.Assets.ToList(),
            UpdatedAt = DateTime.UtcNow
        };

        if (entity.Assets.Sum(a => a.Weight) != 1m)
            throw new InvalidOperationException("A soma dos pesos deve ser 1.0");

        _db.Portfolios.Add(entity);
        await _db.SaveChangesAsync();

        return ToDto(entity);
    }

    public async Task<IEnumerable<PortfolioResponseDto>> GetByUserAsync(Guid userId)
    {
        var list = await _db.Portfolios
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.UpdatedAt)
            .ToListAsync();

        return list.Select(ToDto);
    }

    public async Task<PortfolioResponseDto?> UpdateAsync(Guid id, PortfolioUpdateDto dto)
    {
        var entity = await _db.Portfolios.FirstOrDefaultAsync(p => p.Id == id);
        if (entity is null) return null;

        entity.Name = dto.Name;
        entity.TotalAmount = dto.TotalAmount;
        entity.Assets = dto.Assets;
        entity.UpdatedAt = DateTime.UtcNow;

        if (entity.Assets.Sum(a => a.Weight) != 1m)
            throw new InvalidOperationException("A soma dos pesos deve ser 1.0");

        await _db.SaveChangesAsync();
        return ToDto(entity);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _db.Portfolios.FirstOrDefaultAsync(p => p.Id == id);
        if (entity is null) return false;

        _db.Portfolios.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }

    private static PortfolioResponseDto ToDto(Portfolio p) =>
        new(p.Id, p.UserId, p.Name, p.TotalAmount, p.Assets, p.UpdatedAt);
}
