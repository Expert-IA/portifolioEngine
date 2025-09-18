using System.ComponentModel.DataAnnotations;

namespace InvestorTrust.Contracts.Portfolios;

/// <summary>
/// Payload para criação de uma carteira.
/// </summary>
public sealed record PortfolioCreateDto(
    [Required] Guid UserId,
    [Required, StringLength(160)] string Name,
    [Range(0, double.MaxValue)] decimal TotalAmount,
    [MinLength(1)] IReadOnlyList<AssetDto> Assets
);