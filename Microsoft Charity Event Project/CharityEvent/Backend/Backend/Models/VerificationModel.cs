using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class VerificationModel 
    {
        public string Email { get; set; }
        public string VerificationCode { get; set; }
    }
}
