using RoyaleTrackerAPI.Models;
using RoyaleTrackerClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoyaleTrackerAPI
{
    public class CustomAuthenticationManager : ICustomAuthenticationManager
    {
        //I Don't fully understand this, but Authentication is essential
        public User GetUserByToken(string token, TRContext context)
        {
            User userToReturn = new User() {Token = token };
            userToReturn = context.Users.Where(u => u.Token == token).FirstOrDefault();

            if(userToReturn!=null)
                userToReturn.Password = null;

            return userToReturn; 
        }

        public string Authenticate(string username, string password, TRContext context)
        {
            User user = context.Users.Where(u => u.Username == username && u.Password == password).FirstOrDefault();
            

            //if no user is retrieved
            if(user == null)
            {
                return null;
            }

            if(user.Token == null)
            {  
                user.Token = Guid.NewGuid().ToString();
                context.SaveChanges();
            }
            return user.Token;
        }
    }
}
