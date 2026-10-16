using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.Dto
{
    public class RegisterDto
    {
        public string email { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }
    }
}
