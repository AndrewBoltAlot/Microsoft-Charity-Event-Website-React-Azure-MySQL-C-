using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class LoginModel : ILogin
    {
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
