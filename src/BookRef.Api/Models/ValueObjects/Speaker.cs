namespace BookRef.Api.Models.ValueObjects
{
    public class Speaker
    {
        protected Speaker() { }
        public Speaker(string name)
        {
            Name = name;
        }
        public long Id { get; private set; }
        public string Name { get; private set; }
    }
}
