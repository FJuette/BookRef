namespace BookRef.Api.Models.ValueObjects
{
    public class Person
    {
        protected Person() { }
        public Person(string name)
        {
            Name = name;
        }
        public long Id { get; private set; }
        public string Name { get; private set; }
    }
}
