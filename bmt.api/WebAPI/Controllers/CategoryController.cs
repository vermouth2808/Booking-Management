using Core.Application.Interfaces;
using Core.Shared.DTOs.Request.Category;
using Core.Shared.DTOs.Response.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategorysController : ControllerBase
{
    private readonly ICategoryService<CategoryRes> _serviceCategory;

    public CategorysController(ICategoryService<CategoryRes> serviceCategory)
    {
        _serviceCategory = serviceCategory;
    }
    /// <summary>
    /// search Category
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpGet("ReadAll")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryRes>))]
    [Authorize(Roles = "client,admin")]
    public async Task<IActionResult> ReadAllCategory()
    {
        var result = await _serviceCategory.ReadAllCategory();
        if (result is null || !result.IsSuccess)
        {
            return NotFound();
        }
        return Ok(result.Data);
    }
    /// <summary>
    /// Create a Category
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost("Create")]
    [ProducesResponseType(200, Type = typeof(bool))]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> CreateCategory(CreateCategoryReq req)
    {
        if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
        {
            return Unauthorized("Invalid User ID in token.");
        }

        var result = await _serviceCategory.CreateCategory(req, userId);
        if (result is null || !result.IsSuccess)
        {
            return NotFound();
        }
        return Ok(result.Data);
    }

    /// <summary>
    /// Update a Category
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPut("Update")]
    [ProducesResponseType(200, Type = typeof(bool))]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> UpdateCategory(UpdateCategoryReq req)
    {
        if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
        {
            return Unauthorized("Invalid User ID in token.");
        }

        var result = await _serviceCategory.UpdateCategory(req, userId);
        if (result is null || !result.IsSuccess)
        {
            return NotFound();
        }
        return Ok(result.Data);
    }
    /// <summary>
    /// read by id Category
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("GetById/{id}")]
    [ProducesResponseType(200, Type = typeof(CategoryRes))]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _serviceCategory.GetCategoryById(id);

        if (result == null || !result.IsSuccess)
        {
            return NotFound();
        }

        return Ok(result.Data);
    }
    /// <summary>
    /// Remove a Category
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("Delete/{id}")]
    [ProducesResponseType(200, Type = typeof(bool))]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
        {
            return Unauthorized("Invalid User ID in token.");
        }
        var result = await _serviceCategory.DeleteCategory(id, userId);

        if (result == null || !result.IsSuccess)
        {
            return NotFound();
        }

        return Ok(result.Data);
    }

}
