using System;
using System.Collections.Generic;
using System.Text;
using Application.Interfaces;
using Domain.Entities;
using Domain.Entities.Dto;

namespace Infrastructure.Services
{
    public class AuthService : IAuthInterface
    {
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
