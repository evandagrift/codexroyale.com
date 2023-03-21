
using CodexRoyaleClasses.Models;

namespace CodexRoyaleClasses
{
    public interface ICustomAuthenticationManager
    {
        string Authenticate(string username, string password, TRContext context);
        User GetUserByToken(string token, TRContext context);

    }
}
