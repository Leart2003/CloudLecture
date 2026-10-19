using System;
using System.Collections.Generic;
using System.Text;
using Application.Dto;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.DbContext;
using Microsoft.AspNetCore.Identity;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class AuthService : IAuthInterface
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly PasswordHasher<User> _hasher = new();

     

        public AuthService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
          
        }
        public async Task<User?> Login(LoginDto dto)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == dto.email);

            if (user == null)
               { return null; }
            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
                return null;

            return user;
        }

        public async Task<bool> Register(RegisterDto dto)
        {

            if (dto.Role != "Professor" && dto.Role != "Student")
                return false;

            if (await _dbContext.Users.AnyAsync(u => u.Email == dto.email))
                return false;

            var user = new User
            {
                Email = dto.email,
                Role = dto.Role
            };

            user.PasswordHash = _hasher.HashPassword(user, dto.Password);

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return true;


        }
    }
}
