using CodexRoyaleClasses.Models;
using CodexRoyaleClasses.Repos;

namespace CodexRoyaleClasses
{
    public class CustomAuthenticationManager
    {

        //I Don't fully understand this, but Authentication is essential
        public User GetUserByToken(string token, TRContext context)
        {
            User userToReturn = new User() { BearerToken = token };
            userToReturn = context.Users.Where(u => u.BearerToken == token && u.EmailVerified == true).FirstOrDefault();

            return userToReturn;
        }



        public User VerifyUser(string verificationCode, UsersRepo usersRepo, TRContext context)
        {
            //gets user with this verification code
            User fetchedUser = context.Users.Where(u => u.EmailVerificationCode == verificationCode).FirstOrDefault();

            //makes sure a user with this verification code exists
            if (fetchedUser != null)
            {
                //calculates how much time  left the user has to verify their account
                TimeSpan timeLeftToVerify = DateTime.Parse(fetchedUser.EmailVerificationTokenExpirationDate) - DateTime.UtcNow;

                if (timeLeftToVerify.TotalSeconds > 0)
                {
                    //removes the verification code and sets the user as verified
                    fetchedUser.EmailVerificationCode = null;
                    fetchedUser.EmailVerificationTokenExpirationDate = null;
                    fetchedUser.EmailVerified = true;

                    //creates a unique string for the user's bearer token
                    fetchedUser.BearerToken = Guid.NewGuid().ToString() + Guid.NewGuid()
                        + Guid.NewGuid().ToString();

                    //Verifies the users Tag as well as 
                    fetchedUser = usersRepo.GetPlayerDetails(fetchedUser);
                    context.SaveChanges();

                    //returns the user with sensitive data removed, but includes their bearer token for API access
                    return SanitizeUser(fetchedUser);
                }
            }
            //if the verification code doesn't exist or has expired return null
            return null;
        }

        /*
        public string SendPasswordReset(string userEmail, UsersRepo usersRepo, TRContext context, EmailSender emailSender)
        {
            //gets the user with the recieved email
            User fetchedUser = context.Users.Where(u => u.Email == userEmail).FirstOrDefault();

            //if such a user exists
            if (fetchedUser != null)
            {
                if (fetchedUser.PasswordResetCode == null)
                {

                    //assigns a password reset code and expiration time on said code
                    fetchedUser.PasswordResetCode = Guid.NewGuid().ToString() + Guid.NewGuid()
                            + Guid.NewGuid().ToString();
                    fetchedUser.PasswordResetCodeExpirationDate = DateTime.UtcNow.AddDays(7).ToString();
                    context.SaveChanges();
                }

                emailSender.SendPasswordResetAsync(userEmail, fetchedUser.Username, fetchedUser.PasswordResetCode);



                return fetchedUser.Username;

            }
            else return "";
        }

        */
        public User ResetUserPassword(string password, string resetCode, UsersRepo usersRepo, TRContext context)
        {
            User fetchedUser = context.Users.Where(u => u.PasswordResetCode == resetCode && u.PasswordResetCode != null).FirstOrDefault();

            if (fetchedUser != null)
            {
                //calculates how much time  left the user has to reset their password
                TimeSpan timeLeftToReset = DateTime.Parse(fetchedUser.PasswordResetCodeExpirationDate) - DateTime.UtcNow;

                if (timeLeftToReset.TotalSeconds > 0)
                {
                    //encrypts the users new password before saving it
                    fetchedUser.Password = BCrypt.Net.BCrypt.HashPassword(password);
                    fetchedUser.PasswordResetCode = null;
                    fetchedUser.PasswordResetCodeExpirationDate = null;


                    context.SaveChanges();
                    return SanitizeUser(fetchedUser);
                }
            }
            return null;
        }

        /*
        //for users to create accounts
        public string CreateAccount(User user, EmailSender emailSender, UsersRepo usersRepo, TRContext context)
        {
            //tries to fetch any saved users with given details
            User fetchedUser = context.Users.Where(u => u.Username == user.Username || u.Email == user.Email).FirstOrDefault();

            //if this user already exists returns Null
            if (fetchedUser != null)
            {
                //if this user already exists and is verified returns such
                if (fetchedUser.EmailVerified == true)
                {
                    return "account-already-exists";
                }

            }

            //Verifies the users Tag as well as their clan
            user = usersRepo.GetPlayerDetails(user);

            //make sure all neccessary fields are filled
            if (user.Username != null && user.Password.Length >= 8 && user.Email != null && user.Tag != null)
            {
                try
                {
                    if (fetchedUser == null)
                    {
                        //encrypts the users password before saving it
                        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

                        //if it is the first user added it will be assigned as an Admin
                        //otherwise sets as a general user
                        if (context.Users.Count() == 0) { user.Role = "Admin"; }
                        else { user.Role = "User"; }

                        //creates verification code to be sent to the user's email
                        user.EmailVerificationCode = Guid.NewGuid().ToString() + Guid.NewGuid()
                            + Guid.NewGuid().ToString();

                        //create expiration date on Auth Token ~1 day
                        user.EmailVerificationTokenExpirationDate = DateTime.UtcNow.AddDays(1).ToString();

                        //adds this new user to the database
                        context.Users.Add(user);

                        TrackedPlayer trackedPlayer = context.TrackedPlayers.Where(t => t.Tag == user.Tag).FirstOrDefault();


                        //adding battles is done in the auto update thread to handle concurrency errors
                        //adds the player to have their data tracked
                        if (trackedPlayer == null)
                        {
                            context.TrackedPlayers.Add(new TrackedPlayer { Tag = user.Tag, Priority = "high" });

                        }
                        context.SaveChanges();
                    }
                    else user = fetchedUser;


                    //TODO:FIX SENDGrID
                    //commented out until sendgrid fixed
                    emailSender.SendEmailVerificationAsync(user.Email, user.EmailVerificationCode);


                    //temporarily here until sendgrid working again
                    //VerifyUser(user.EmailVerificationCode, usersRepo, context);



                    //confirms user was created
                    return "verification-link-sent";
                }
                catch { return "failed-to-create"; }
            }
            return "invalid-credentials";

        }
        //User update/password reset for general users
        public User UpdateUserSetting(User user, string newPassword, TRContext context, Client client)
        {
            UsersRepo userRepo = new UsersRepo(client, context);

            User fetchedUser = context.Users.Find(user.Username);

            if (fetchedUser != null)
            {

                if (user.Tag != "")
                {
                    fetchedUser.Tag = user.Tag;
                    fetchedUser = userRepo.GetPlayerDetails(fetchedUser);

                }


                if (newPassword != "")
                {
                    fetchedUser = Login(user, context);

                    if (newPassword.Length > 9)
                    {
                        //encrypts the users password before saving it
                        fetchedUser.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
                    }


                    context.SaveChanges();

                    return SanitizeUser(fetchedUser);

                }

                context.SaveChanges();
                return SanitizeUser(fetchedUser);
            }

            else return null;


        }
        */

        public User Login(User submittedUser, TRContext context)
        {
            //fetches user with given username from the database
            User fetchedUser = context.Users.Where(u => u.Username == submittedUser.Username).FirstOrDefault();

            if (fetchedUser != null && submittedUser != null)
            {
                //makes sure both users have a password and the user that is trying to log in is verified
                if (fetchedUser.Password != null && submittedUser.Password != null && fetchedUser.EmailVerified == true)
                {
                    //Checks if the entered password is correct
                    if (BCrypt.Net.BCrypt.Verify(submittedUser.Password, fetchedUser.Password))
                    {
                        //returns sanitized user if the username and password are correct
                        return SanitizeUser(fetchedUser);

                    }
                }

            }

            //if the username or password are incorrect or the user is unverified returns null
            return null;
        }

        //removes all sensitive fields except email
        private User SanitizeUser(User user)
        {
            user.Password = null;
            user.PasswordResetCode = null;
            user.PasswordResetCodeExpirationDate = null;
            user.EmailVerificationCode = null;
            user.EmailVerificationTokenExpirationDate = null;

            return user;
        }

    }
}