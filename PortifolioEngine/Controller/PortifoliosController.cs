// Api/Controllers/PortfoliosController.cs

using InvestorTrust.Contracts.Portfolios;
using Microsoft.AspNetCore.Mvc;
using PortifolioEngine.Domain.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class PortfoliosController : ControllerBase
{
    private readonly IPortfolioService _service;
    public PortfoliosController(IPortfolioService service) => _service = service;

    [HttpPost]
    public async Task<ActionResult<PortfolioResponseDto>> Create([FromBody] PortfolioCreateDto dto)
    {
        try
        {
            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByUser), new { userId = result.UserId }, result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("user/{userId:guid}")]
    public async Task<ActionResult<IEnumerable<PortfolioResponseDto>>> GetByUser(Guid userId)
    {
        var result = await _service.GetByUserAsync(userId);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<PortfolioResponseDto>> Update(Guid id, [FromBody] PortfolioUpdateDto dto)
    {
        try
        {
            var result = await _service.UpdateAsync(id, dto);
            if (result is null) return NotFound();
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
    
}