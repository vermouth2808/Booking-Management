using Core.Application.Interfaces;
using Core.Shared.DTOs.Request.ShowTime;
using Core.Shared.DTOs.Response.ShowTime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ShowTimesController : ControllerBase
{
    private readonly IShowTimeService<ShowTimeRes> _serviceShowTime;

    public ShowTimesController(IShowTimeService<ShowTimeRes> serviceShowTime)
    {
        _serviceShowTime = serviceShowTime;
    }
    /// <summary>
    /// search ShowTime
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost("SearchShowTime")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<SearchShowTimeReq>))]
    [Authorize(Roles = "client,admin")]
    public async Task<IActionResult> SearchShowTime(SearchShowTimeReq req)
    {
        var result = await _serviceShowTime.SearchShowTime(req);
        if (result is null || !result.IsSuccess)
        {
            return NotFound();
        }
        return Ok(result.Data);
    }
    /// <summary>
    /// Create a ShowTime
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost("CreateShowTime")]
    [ProducesResponseType(200, Type = typeof(bool))]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CreateShowTime(CreateShowTimeReq req)
    {
        if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
        {
            return Unauthorized("Invalid User ID in token.");
        }

        var result = await _serviceShowTime.CreateShowTime(req, userId);
        if (result is null || !result.IsSuccess)
        {
            return NotFound();
        }
        return Ok(result.Data);
    }

    /// <summary>
    /// Update a ShowTime
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPut("UpdateShowTime")]
    [ProducesResponseType(200, Type = typeof(bool))]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> UpdateShowTime(UpdateShowTimeReq req)
    {
        if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
        {
            return Unauthorized("Invalid User ID in token.");
        }

        var result = await _serviceShowTime.UpdateShowTime(req, userId);
        if (result is null || !result.IsSuccess)
        {
            return NotFound();
        }
        return Ok(result.Data);
    }
    /// <summary>
    /// read by id ShowTime
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("GetById/{id}")]
    [ProducesResponseType(200, Type = typeof(ShowTimeRes))]
    [Authorize(Roles = "client,admin")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _serviceShowTime.GetShowTimeById(id);

        if (result == null || !result.IsSuccess)
        {
            return NotFound();
        }

        return Ok(result.Data);
    }
    /// <summary>
    /// Remove a ShowTime
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(200, Type = typeof(bool))]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteShowTime(int id)
    {
        if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
        {
            return Unauthorized("Invalid User ID in token.");
        }
        var result = await _serviceShowTime.DeleteShowTime(id, userId);

        if (result == null || !result.IsSuccess)
        {
            return NotFound();
        }

        return Ok(result.Data);
    }

}
