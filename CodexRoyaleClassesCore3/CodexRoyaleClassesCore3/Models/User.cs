using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CodexRoyaleClassesCore3.Models
{
    public class User
    {
        [Key]
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool EmailVerified { get; set; }

        public string Tag { get; set; }
        public string ClanTag { get; set; }
        public string Role { get; set; }

        public string BearerToken { get; set; }

        public string EmailVerificationCode { get; set; }
        public string EmailVerificationTokenExpirationDate { get; set; }

        public string PasswordResetCode { get; set; }
        public string PasswordResetCodeExpirationDate { get; set; }

    }
}