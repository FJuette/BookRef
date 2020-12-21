using System;

namespace BookRef.Api.Models
{
    public class User : EntityBase
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string EMail { get; set; }
        public Guid PersonalLibraryId { get; set; }

        public override string ToString()
        {
            return $"{Username}";
        }
    }
}
