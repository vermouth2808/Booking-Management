using Core.Domain.Entities;
using Core.Infrastructure.Repositories.Interfaces;
using Core.Shared.Common.Models;
using Core.Shared.DTOs.Request.Auth;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Core.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly BookMovieTicketContext _context;

        public AuthRepository(BookMovieTicketContext context)
        {
            _context = context;
        }

        public async Task<Result<bool>> RegisterClient(UserRegisterReq req)
        {
            try
            {
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.UserName == req.UserName);

                if (existingUser != null)
                {
                    return Result<bool>.Failure("Username is already taken.");
                }

                var user = new User
                {
                    UserName = req.UserName,
                    FullName = req.FullName,
                    PassWord = HashPassword(req.PassWord),
                    IsActive = true,
                    RoleId = 6,//user client
                    IsOnline = true,//online
                    IsSuperAdmin = false,
                    CreatedDate = DateTime.UtcNow,
                    CreatedUserId = 1,
                    IsDeleted = false,
                };

                var customer = new Customer
                {
                    CustomerName = req.FullName,
                    Email = req.Email,
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false,
                };
                _context.Users.Add(user);
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error: {ex.Message}");
            }
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }

}
