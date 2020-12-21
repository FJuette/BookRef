namespace BookRef.Api.Models
{
    public class Note : EntityBase
    {
        protected Note() { }
        public Note(string content, ShareType shareType = ShareType.None)
        {
            Content = content;
            ShareType = shareType;
        }
        public string Content { get; private set; }
        public ShareType ShareType { get; private set; }
    }

    public enum ShareType
    {
        None,
        Friends,
        All
    }
}
