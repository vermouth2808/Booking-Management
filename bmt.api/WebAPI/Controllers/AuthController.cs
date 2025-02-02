using Core.Domain.Entities;
using Core.Shared.DTOs.Auth.Request;
using Core.Shared.DTOs.Auth.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Core.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly BookMovieTicketContext _context;

        public AuthController(IConfiguration config, BookMovieTicketContext context)
        {
            _config = config;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterReq req)
        {
            var user = new User
            {
                UserName = req.UserName,
                FullName = req.FullName,
                PassWord = HashPassword(req.PassWord),
                IsActive = true,
                CreatedDate = DateTime.Now,
                CreatedUserId = 1 
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "User registered successfully" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginReq req)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == req.Username);
            if (user == null || !VerifyPassword(req.Password, user.PassWord))
            {
                return Unauthorized(new { Message = "Invalid username or password" });
            }

            var token = await GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken(user.UserId);

            return Ok(new AuthRes { Token = token, RefreshToken = refreshToken.Token });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenReq req)
        {
            var refreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == req.RefreshToken &&
                                           !rt.IsUsed.GetValueOrDefault() &&
                                           !rt.IsRevoked.GetValueOrDefault());

            if (refreshToken == null || refreshToken.ExpiryDate < DateTime.UtcNow)
            {
                return Unauthorized(new { Message = "Invalid or expired refresh token" });
            }

            // Lấy thông tin user từ token
            var user = await _context.Users.FindAsync(refreshToken.UserId);
            if (user == null)
            {
                return Unauthorized(new { Message = "User not found" });
            }

            // Đánh dấu token đã sử dụng
            refreshToken.IsUsed = true;
            _context.RefreshTokens.Update(refreshToken);
            await _context.SaveChangesAsync();

            // Tạo access token và refresh token mới
            var newAccessToken = await GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken(user.UserId);

            return Ok(new AuthRes { Token = newAccessToken, RefreshToken = newRefreshToken.Token });
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            var Role = await GetRoleUser(user.RoleId);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier , user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, Role.RoleName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:ExpiryInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private RefreshToken GenerateRefreshToken(int userId)
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                IsUsed = false,
                IsRevoked = false,
                UserId = userId
            };

            _context.RefreshTokens.Add(refreshToken);
            _context.SaveChangesAsync(); 

            return refreshToken;
        }


        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return HashPassword(password) == hashedPassword;
        }

        private async Task<Role> GetRoleUser(int roleId)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == roleId);
        }



    }
}