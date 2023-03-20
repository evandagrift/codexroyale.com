using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodexRoyaleClassesCore3.Models.Email
{
    public class AuthMessageSenderOptions
    {
        public string EmailFrom { get; set; }
        public string SendGridUser { get; set; }
        public string SendGridKey { get; set; }
    }
}
