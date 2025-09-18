// src/InvestorTrust.Contracts/Portfolios/AssetDto.cs
using System.ComponentModel.DataAnnotations;

namespace InvestorTrust.Contracts.Portfolios;

/// <summary>
/// Representa um ativo dentro da carteira (usado em criação/atualização e resposta).
/// </summary>
public sealed record AssetDto(
    [Required, StringLength(40)] string Ticker,
    [Required, StringLength(40)] string Type,
    [Range(0, 1)] decimal Weight
);