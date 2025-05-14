using Core.Application.Interfaces;
using Core.Shared.DTOs.Request.FoodCombo;
using Core.Shared.DTOs.Response.FoodCombo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class FoodCombosController : ControllerBase
{
    private readonly IFoodComboService<FoodComboRes> _serviceFoodCombo;

    public FoodCombosController(IFoodComboService<FoodComboRes> serviceFoodCombo)
    {
        _serviceFoodCombo = serviceFoodCombo;
    }
    /// <summary>
    /// search FoodCombo
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpGet("ReadAll")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<FoodComboRes>))]
    [Authorize(Roles = "client,admin")]
    public async Task<IActionResult> ReadAllFoodCombo()
    {
        var result = await _serviceFoodCombo.ReadAllFoodCombo();
        if (result is null || !result.IsSuccess)
        {
            return NotFound();
        }
        return Ok(result.Data);
    }
    /// <summary>
    /// Create a FoodCombo
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost("Create")]
    [ProducesResponseType(200, Type = typeof(bool))]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CreateFoodCombo(CreateFoodComboReq req)
    {
        if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
        {
            return Unauthorized("Invalid User ID in token.");
        }

        var result = await _serviceFoodCombo.CreateFoodCombo(req, userId);
        if (result is null || !result.IsSuccess)
        {
            return NotFound();
        }
        return Ok(result.Data);
    }

    /// <summary>
    /// Update a FoodCombo
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPut("Update")]
    [ProducesResponseType(200, Type = typeof(bool))]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> UpdateFoodCombo(UpdateFoodComboReq req)
    {
        if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
        {
            return Unauthorized("Invalid User ID in token.");
        }

        var result = await _serviceFoodCombo.UpdateFoodCombo(req, userId);
        if (result is null || !result.IsSuccess)
        {
            return NotFound();
        }
        return Ok(result.Data);
    }
    /// <summary>
    /// read by id FoodCombo
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("GetById/{id}")]
    [ProducesResponseType(200, Type = typeof(FoodComboRes))]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _serviceFoodCombo.GetFoodComboById(id);

        if (result == null || !result.IsSuccess)
        {
            return NotFound();
        }

        return Ok(result.Data);
    }
    /// <summary>
    /// Remove a FoodCombo
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("Delete/{id}")]
    [ProducesResponseType(200, Type = typeof(bool))]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteFoodCombo(int id)
    {
        if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
        {
            return Unauthorized("Invalid User ID in token.");
        }
        var result = await _serviceFoodCombo.DeleteFoodCombo(id, userId);

        if (result == null || !result.IsSuccess)
        {
            return NotFound();
        }

        return Ok(result.Data);
    }

}
