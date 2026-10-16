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
        public Task<User?> Login(LoginDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Register(RegisterDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
