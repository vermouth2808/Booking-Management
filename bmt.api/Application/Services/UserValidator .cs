using Core.Application.Interfaces;
using Core.Domain.Entities;
using Core.Shared.Common.Models;
using Core.Shared.DTOs.Request.Auth;
using Microsoft.EntityFrameworkCore;

namespace Core.Application.Services
{
    public class UserValidator : IUserValidator
    {
        private readonly BookMovieTicketContext _context;

        public UserValidator(BookMovieTicketContext context)
        {
            _context = context;
        }

        public async Task<Result<bool>> ValidateUser(UserRegisterReq req)
        {
            if (string.IsNullOrEmpty(req.UserName) || string.IsNullOrEmpty(req.PassWord))
            {
                return Result<bool>.Failure("Username and password are required.");
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == req.UserName);
            if (existingUser != null)
            {
                return Result<bool>.Failure("Username is already taken.");
            }

            return Result<bool>.Success(true);
        }
    }

}
