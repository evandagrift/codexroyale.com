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

            if (userToReturn != null)
                userToReturn.Password = null;

            return userToReturn;
        }
        //for users to create accounts
        public User CreateAccount(User user,UsersRepo usersRepo, TRContext context)
        {
            //fetches a user by email or username if there is a match
            var checkValid = context.Users.Where(u => u.Username == user.Username || u.Email == user.Email).FirstOrDefault();

            //if there are no users with those credentials checkValid will be null
            if (checkValid == null)
            {
                //make sure all neccessary fields are filled
                if (user.Username != null && user.Password.Length >= 8 && user.Email != null)
                {
                    user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

                    if (context.Users.Count() == 0) { user.Role = "Admin"; }
                    else { user.Role = "User"; }

                    user.Token = Guid.NewGuid().ToString() + Guid.NewGuid().ToString();

                    user = usersRepo.SaveAllByUser(user);
                    context.Add(user);
                    context.SaveChanges();

                    return user;
                }
                //if a field isn't valid
                return null;
            }
            //if a user is already using these credentials
            return null;
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

                        return fetchedUser;
                    }
                }
            }
            return null;
        }
        public Player Update(User user, TRContext context)
        {



            
            return null;
        }

    }
}
