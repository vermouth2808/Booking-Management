using Core.Application.Interfaces;
using Core.Shared.DTOs.Request.FoodOrder;
using Core.Shared.DTOs.Response.FoodOrder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class FoodOrdersController : ControllerBase
{
    private readonly IFoodOrderService<FoodOrderRes> _serviceFoodOrder;

    public FoodOrdersController(IFoodOrderService<FoodOrderRes> serviceFoodOrder)
    {
        _serviceFoodOrder = serviceFoodOrder;
    }
    /// <summary>
    /// search FoodOrder
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost("SearchFoodOrder")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<FoodOrderSearchRes>))]
    [Authorize(Roles = "client,admin")]
    public async Task<IActionResult> SearchFoodOrder(SearchFoodOrderReq req)
    {
        var result = await _serviceFoodOrder.SearchFoodOrder(req);
        if (result is null || !result.IsSuccess)
        {
            return NotFound();
        }
        return Ok(result.Data);
    }
    /// <summary>
    /// Create a FoodOrder
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost("CreateFoodOrder")]
    [ProducesResponseType(200, Type = typeof(bool))]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CreateFoodOrder(CreateFoodOrderReq req)
    {
        if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
        {
            return Unauthorized("Invalid User ID in token.");
        }

        var result = await _serviceFoodOrder.CreateFoodOrder(req, userId);
        if (result is null || !result.IsSuccess)
        {
            return NotFound();
        }
        return Ok(result.Data);
    }

    /// <summary>
    /// Update a FoodOrder
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPut("UpdateFoodOrder")]
    [ProducesResponseType(200, Type = typeof(bool))]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> UpdateFoodOrder(UpdateFoodOrderReq req)
    {
        if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
        {
            return Unauthorized("Invalid User ID in token.");
        }

        var result = await _serviceFoodOrder.UpdateFoodOrder(req, userId);
        if (result is null || !result.IsSuccess)
        {
            return NotFound();
        }
        return Ok(result.Data);
    }
    /// <summary>
    /// read by id FoodOrder
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("GetById/{id}")]
    [ProducesResponseType(200, Type = typeof(FoodOrderRes))]
    [Authorize(Roles = "client,admin")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _serviceFoodOrder.GetFoodOrderById(id);

        if (result == null || !result.IsSuccess)
        {
            return NotFound();
        }

        return Ok(result.Data);
    }
    /// <summary>
    /// Remove a FoodOrder
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(200, Type = typeof(bool))]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteFoodOrder(int id)
    {
        if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
        {
            return Unauthorized("Invalid User ID in token.");
        }
        var result = await _serviceFoodOrder.DeleteFoodOrder(id, userId);

        if (result == null || !result.IsSuccess)
        {
            return NotFound();
        }

        return Ok(result.Data);
    }

}
