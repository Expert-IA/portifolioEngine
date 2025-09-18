// Domain/Entities/Portfolio.cs
using InvestorTrust.Contracts.Portfolios;

namespace PortifolioEngine.Domain.Entities;

/// <summary>
/// Entidade de Portfólio persistida no banco.
/// </summary>
public class Portfolio
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public List<AssetDto> Assets { get; set; } = new();
    public DateTime UpdatedAt { get; set; }
}