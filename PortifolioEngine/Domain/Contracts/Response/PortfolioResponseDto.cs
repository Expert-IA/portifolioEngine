// src/InvestorTrust.Contracts/Portfolios/PortfolioResponseDto.cs
namespace InvestorTrust.Contracts.Portfolios;

/// <summary>
/// DTO de resposta para operações de carteira.
/// </summary>
public sealed record PortfolioResponseDto(
    Guid Id,
    Guid UserId,
    string Name,
    decimal TotalAmount,
    List<AssetDto> Assets,
    DateTime UpdatedAt
);