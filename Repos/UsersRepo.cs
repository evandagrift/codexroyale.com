using RoyaleTrackerAPI.Models;
using RoyaleTrackerClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCrypt;

namespace RoyaleTrackerAPI.Repos
{
    public class UsersRepo
    {
        //DB Context
        private TRContext context;
        private Client client;

        //constructor, connects DB Context for usage
        public UsersRepo(Client c, TRContext ct) { context = ct; client = c; ; }
        /*
                //for users to create accounts
                public User CreateAccount(User user)
                {
                    //fetches a user by email or username if there is a match
                    var checkValid = context.Users.Where(u => u.Username == user.Username || u.Email == user.Email).FirstOrDefault();

                    //if there are no users with those credentials checkValid will be null
                    if(checkValid == null)
                    {
                        //make sure all neccessary fields are filled
                        if(user.Username != null && user.Password.Length > 8 && user.Email != null)
                        {
                            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                            context.Users.Add(user);
                            context.SaveChanges();
                            return user;
                        }
                        //if a field isn't valid
                        return null;
                    }
                    //if a user is already using these credentials
                    return null;
                }


                public string Login(User user)
                {
                    User fetchedUser = context.Users.Where(u => u.Username == user.Username).FirstOrDefault();
                    if (fetchedUser != null)
                    {
                        //if the password is a match
                        if (BCrypt.Net.BCrypt.Verify( user.Password, fetchedUser.Password))
                        {
                            //if no user is retrieved
                            if (user == null)
                            {
                                return null;
                            }

                            if (user.Token == null)
                            {
                                user.Token = Guid.NewGuid().ToString();
                                context.SaveChanges();
                            }
                            return user.Token;
                        }
                    }
                    return null;
                }

                */
        //create
        public void AddUser(User user)
        {
            if (!context.Users.Any(u => u.Username == user.Username))
                {
                context.Add(user);
                context.SaveChanges();
            }
        }

        //read by Username
        public User GetUserByUsername(string username) { return context.Users.Where(u => u.Username == username).FirstOrDefault(); }

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

            if (user != null)
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
            if (userToUpdate != null)
            {
                userToUpdate.Role = (user.Role != null) ? user.Role : userToUpdate.Role;
                userToUpdate.Email = (user.Email != null) ? user.Email : userToUpdate.Email;
                userToUpdate.Tag = (user.Tag != null) ? user.Tag : userToUpdate.Tag;
                userToUpdate.ClanTag = (user.ClanTag != null) ? user.ClanTag : userToUpdate.ClanTag;
                userToUpdate.Password = (user.Password != null) ? user.Password : userToUpdate.Password;

                context.SaveChanges();
            }
        }


        public User SaveAllByUser(User user)
        {
            //check if the inserted tag is correct, and if so. get clan tag as well
            ClansRepo clansRepo = new ClansRepo(client, context);
            BattlesRepo battleRepo = new BattlesRepo(client, context);
            PlayersRepo playerRepo = new PlayersRepo(client, context);
            if (user.Tag != null)
            {
                try
                {
                    Player player = playerRepo.GetOfficialPlayer(user.Tag).Result;
                    if (player != null)
                    {
                        context.Players.Add(player);

                        List<Battle> pBattles;
                        //TODO:get save user and their battles to DB
                        //fetches the current player battles from the official DB
                        pBattles = battleRepo.GetOfficialPlayerBattles(player.Tag).Result;


                        //adds new fetched battles to the DB and gets a count of added lines
                        battleRepo.AddBattles(pBattles);

                        if (player.ClanTag != "")
                        {
                            //TODO:Save Clan to DB
                            //gets current clan with Tag
                            Clan clan = clansRepo.GetOfficialClan(player.ClanTag).Result;

                            //saves clan data to DB
                            context.Clans.Add(clan);
                            context.SaveChanges();
                            user.ClanTag = player.ClanTag;
                        }
                    }
                    else
                    {

                        user.Tag = null;
                        user.ClanTag = null;
                    }
                }
                catch { return null; }
            }
            return user;
        }




    }
}
