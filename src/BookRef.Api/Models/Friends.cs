namespace BookRef.Api.Models
{
    public class Friends
    {
        public User SourceUser { get; set; }
        public long SourceUserId { get; set; }

        public User FriendUser { get; set; }
        public long FriendUserId { get; set; }
    }
}
