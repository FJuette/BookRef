using System;

namespace BookRef.Api.Models
{
    public class User : EntityBase
    {
        protected User() { }
        public User(string username, string email)
        {
            Username = username;
            EMail = email;
        }

        public User SetPassword(string password)
        {
            Password = BCrypt.Net.BCrypt.HashPassword(password);
            return this;
        }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string EMail { get; private set; }
        public Guid PersonalLibraryId { get; set; }

        public override string ToString()
        {
            return $"{Username}";
        }
    }
}
