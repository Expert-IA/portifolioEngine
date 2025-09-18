using InvestorTrust.Contracts.Portfolios;

namespace PortifolioEngine.Domain.Interfaces;

public interface IPortfolioService
{
    Task<PortfolioResponseDto> CreateAsync(PortfolioCreateDto dto);
    Task<IEnumerable<PortfolioResponseDto>> GetByUserAsync(Guid userId);
    Task<PortfolioResponseDto?> UpdateAsync(Guid id, PortfolioUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
    
}