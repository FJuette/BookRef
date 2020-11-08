namespace BookRef.Api.Models
{
    public class Note : EntityBase
    {
        public string Content { get; set; }
        public ShareType ShareType { get; set; }
    }

    public enum ShareType
    {
        None,
        Friends,
        All
    }
}
