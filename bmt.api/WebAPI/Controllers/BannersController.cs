using Core.Infrastructure.Repositories.Interfaces;
using Core.Shared.DTOs.Request.Banner;
using Core.Shared.DTOs.Response.Banner;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BannersController : ControllerBase
{
    private readonly IBannerRepository<BannerRes> _repositoryBanner;

    public BannersController(IBannerRepository<BannerRes> repositoryBanner)
    {
        _repositoryBanner = repositoryBanner;
    }
    /// <summary>
    /// search Banner
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpGet("ReadAllBanner")]
    [ProducesResponseType(200, Type = typeof(ReadAllBannerRes))]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> SearchBanner()
    {
        var result = await _repositoryBanner.ReadAllBanner();
        if (result is null || !result.IsSuccess)
        {
            return NotFound();
        }
        return Ok(result.Data);
    }
    /// <summary>
    /// Create a Banner
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost("CreateBanner")]
    [ProducesResponseType(200, Type = typeof(bool))]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CreateBanner(CreateBannerReq req)
    {
        if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
        {
            return Unauthorized("Invalid User ID in token.");
        }

        var result = await _repositoryBanner.CreateBanner(req, userId);
        if (result is null || !result.IsSuccess)
        {
            return NotFound();
        }
        return Ok(result.Data);
    }

    /// <summary>
    /// Update a Banner
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPut("UpdateBanner")]
    [ProducesResponseType(200, Type = typeof(bool))]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> UpdateBanner(UpdateBannerReq req)
    {
        if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
        {
            return Unauthorized("Invalid User ID in token.");
        }

        var result = await _repositoryBanner.UpdateBanner(req, userId);
        if (result is null || !result.IsSuccess)
        {
            return NotFound();
        }
        return Ok(result.Data);
    }
    /// <summary>
    /// read by id Banner
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("GetById/{id}")]
    [ProducesResponseType(200, Type = typeof(BannerRes))]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _repositoryBanner.GetBannerById(id);

        if (result == null || !result.IsSuccess)
        {
            return NotFound();
        }

        return Ok(result.Data);
    }
    /// <summary>
    /// Remove a Banner
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(200, Type = typeof(bool))]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteBanner(int id)
    {
        if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
        {
            return Unauthorized("Invalid User ID in token.");
        }
        var result = await _repositoryBanner.DeleteBanner(id, userId);

        if (result == null || !result.IsSuccess)
        {
            return NotFound();
        }

        return Ok(result.Data);
    }

}
