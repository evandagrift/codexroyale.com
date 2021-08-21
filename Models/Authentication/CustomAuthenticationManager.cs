using RoyaleTrackerAPI.Models;
using RoyaleTrackerAPI.Repos;
using RoyaleTrackerClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoyaleTrackerAPI
{
    public class CustomAuthenticationManager
    {

        //I Don't fully understand this, but Authentication is essential
        public User GetUserByToken(string token, TRContext context)
        {
            User userToReturn = new User() { Token = token };
            userToReturn = context.Users.Where(u => u.Token == token).FirstOrDefault();


            return userToReturn;
        }
        //for users to create accounts
        public User CreateAccount(User user,UsersRepo usersRepo, TRContext context)
        {
            if (context.Users.Any(u => u.Username == user.Username || u.Email == user.Email))
            { return null; }
            else
            {
                //make sure all neccessary fields are filled
                if (user.Username != null && user.Password.Length >= 8 && user.Email != null)
                {
                    try
                    {
                        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

                        if (context.Users.Count() == 0) { user.Role = "Admin"; }
                        else { user.Role = "User"; }

                        user.Token = Guid.NewGuid().ToString() + Guid.NewGuid().ToString();

                        user = usersRepo.SaveAllByUser(user);
                        context.Users.Add(user);
                        context.SaveChanges();

                        user.Password = null;
                        return user;
                    }
                    catch { return null; }
                }
                return null;
            }
        }


        public User Login(User user, TRContext context)
        {
            User fetchedUser = context.Users.Where(u => u.Username == user.Username).FirstOrDefault();
            if (fetchedUser != null)
            {
                if (fetchedUser.Password != null)
                {
                    //if the password is a match
                    if (BCrypt.Net.BCrypt.Verify(user.Password, fetchedUser.Password))
                    {
                        user = context.Users.Find(user.Username);
                        //call player to save in DB 
                        //check if player clan is updated


                       //handler.SaveNew(user);

                        //if no user is retrieved
                        if (user == null)
                        {
                            return null;
                        }
                        else
                        {
                            user.Password = null;
                            return user;
                        }

                    }
                }
            }
                return null;
            
        }
        //public Player Update(User user, TRContext context)
        //{



            
        //    return null;
        //}

    }
}
