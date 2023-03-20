using CodexRoyaleClassesCore3.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace CodexRoyaleClassesCore3
{
    public class CustomAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        //I Don't fully understand this, but Authentication is essential
        private readonly CustomAuthenticationManager customAuthenticationManager;
        private readonly TRContext context;

        public CustomAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, TRContext context, CustomAuthenticationManager customAuthenticationManager) :
            base(options, logger, encoder, clock)
        {
            this.context = context;
            this.customAuthenticationManager = customAuthenticationManager;
        }
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Unauthorized");

            string authorizationHeader = Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authorizationHeader))
                return AuthenticateResult.Fail("Unauthorize");

            if (!authorizationHeader.StartsWith("bearer", StringComparison.OrdinalIgnoreCase))
                return AuthenticateResult.Fail("Unauthorize");

            string token = authorizationHeader.Substring("bearer".Length).Trim();

            if (string.IsNullOrEmpty(token))
                return AuthenticateResult.Fail("Unauthorize");

            try
            {
                return ValidateToken(token);
            }
            catch
            {
                return AuthenticateResult.Fail("Unauthorize");
            }

        }

        private AuthenticateResult ValidateToken(string token)
        {
            //retrieve Token from DB
            User validatedUser = customAuthenticationManager.GetUserByToken(token, context);
            if (validatedUser != null)
            {
                if (validatedUser.Username == null || validatedUser.Role == null)
                {
                    return AuthenticateResult.Fail("Unauthorize");
                }

                var claims = new List<Claim>
                {
                    //item1 is name
                    new Claim(ClaimTypes.Name, validatedUser.Username),
                    new Claim(ClaimTypes.Role, validatedUser.Role)
                };

                var identity = new ClaimsIdentity(claims, Scheme.Name);

                //item2 is Role
                var principal = new GenericPrincipal(identity, new[] { validatedUser.Role });

                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);

            }
            else return AuthenticateResult.Fail("Unauthorize");
        }
    }

}