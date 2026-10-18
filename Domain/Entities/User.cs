using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = "Student";
        public string PasswordHash { get; set; } = string.Empty;
    }
}
