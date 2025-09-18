// src/InvestorTrust.Contracts/Portfolios/PortfolioUpdateDto.cs
using System.ComponentModel.DataAnnotations;

namespace InvestorTrust.Contracts.Portfolios;

/// <summary>
/// Payload para atualização de uma carteira existente.
/// </summary>
public sealed record PortfolioUpdateDto(
    [Required, StringLength(160)] string Name,
    [Range(0, double.MaxValue)] decimal TotalAmount,
    [MinLength(1)] IReadOnlyList<AssetDto> Assets
);