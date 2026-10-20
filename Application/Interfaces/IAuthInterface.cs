using Application.Dto;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IAuthInterface
    {
        Task<bool> Register(RegisterDto dto);
        Task<User?> Login(LoginDto dto);

        
    }
}
