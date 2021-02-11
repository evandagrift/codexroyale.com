using RoyaleTrackerAPI.Models;
using RoyaleTrackerClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoyaleTrackerAPI.Repos
{
    public class UsersRepo : IUsersRepo
    {
        //DB Context
        private TRContext context;

        //constructor, connects DB Context for usage
        public UsersRepo(TRContext c) { context = c; }

        //create
        public void AddUser(User user) 
        { 
            context.Add(user);
            context.SaveChanges();
        }

        //read by Username
        public User GetUserByUsername(string username) { return context.Users.Find(username); }

        //read all
        public List<User> GetAllUsers() { return context.Users.ToList(); }


        //delete user by username
        public void DeleteUser(string username)
        {
            //fetches user with given username from DB
            User user = GetUserByUsername(username);

            if (user != null)
            {
                //if a valid user is returned it removes said user from context
                context.Users.Remove(user);
                context.SaveChanges();
                    
            }
        }

        //gets/creates user token by username
        public string GetUserToken(string username)
        {
            //get user by username
            User user = GetUserByUsername(username);

            if(user !=null)
            {
                //if the user exists but doesn't have a token it creates a new token and save the new token to DB
                if (user.Token == null)
                {
                    user.Token = Guid.NewGuid().ToString();
                    context.SaveChanges();
                }
            }
            return user.Token;
        }

        //Updates the user at given username based off submitted User
        public void UpdateUser(User user)
        {
            //fetches user at given username to be updated
            User userToUpdate = GetUserByUsername(user.Username);

            //changes all user fields to the given classes fields
            if(userToUpdate != null)
            {
                userToUpdate.Password = user.Password;
                userToUpdate.Role = user.Role;
                userToUpdate.Token = user.Token;
                context.SaveChanges();
            }
        }
    }
}
