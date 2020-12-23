using System;

namespace BookRef.Api.Models
{
    public class User : EntityBase
    {
        protected User() { }
        public User(string username, string password, string email)
        {
            Username = username;
            Password = password;
            EMail = email;
        }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string EMail { get; private set; }
        public Guid? PersonalLibraryId { get; set; }

        public override string ToString()
        {
            return $"{Username}";
        }
    }
}
