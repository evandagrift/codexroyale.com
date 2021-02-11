using RoyaleTrackerClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoyaleTrackerAPI.Repos
{
    public interface IUsersRepo
    {
        void AddUser(User user); //Create User
        List<User> GetAllUsers(); // Get All Users

        User GetUserByUsername(string username); //Get Single User by ID

        void DeleteUser(string username); //Delete user by ID

        string GetUserToken(string username); //Retrieve Token  
        void UpdateUser(User user);
    }
}
