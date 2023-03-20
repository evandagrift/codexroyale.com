using CodexRoyaleClassesCore3.Models;

namespace CodexRoyaleClassesCore3
{
    public interface ICustomAuthenticationManager
    {
        string Authenticate(string username, string password, TRContext context);
        User GetUserByToken(string token, TRContext context);

    }
}
