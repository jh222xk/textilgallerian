using System;

namespace Domain.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class User
    {

        public String Username { get; set; }

        public String Email { get; set; }

        private String PasswordHash { get; set; }

        /// <summary>
        ///     Hashes the password using the bcrypt algorithm
        /// </summary>
        public String Password
        {
            set
            {
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(value);
            }
        }

        /// <summary>
        ///     Validating the password using the bcrypt algorithm
        /// </summary>
        public Boolean ValidatePassword(String password)
        {
            return BCrypt.Net.BCrypt.Verify(password, PasswordHash);
        }
    }
}
