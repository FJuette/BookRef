using System.Collections.Generic;
using System.Linq;
using BookRef.Api.Models;
using BookRef.Api.Models.Relations;
using BookRef.Api.Models.ValueObjects;
using BookRef.Api.Persistence;

namespace BookRef.Api.Extensions
{
    public class SampleDataSeeder
    {
        private readonly BookRefDbContext _context;

        public SampleDataSeeder(
            BookRefDbContext context) =>
            _context = context;

        public void SeedAll()
        {
            var danAuthor = new Author("Dan O'Riley");
            _context.Attach(danAuthor);
            var hansAuthor = new Author("Hans Rosling");
            _context.Attach(hansAuthor);
            var juliaAuthor = new Author("Julia Shaw");
            _context.Attach(juliaAuthor);

            var categoryGehirn = new Category("Gehirn");
            _context.Attach(categoryGehirn);
            var categoryPsyche = new Category("Psyche");
            _context.Attach(categoryPsyche);
            var categoryBoerse = new Category("Börse");
            _context.Attach(categoryBoerse);

            var charlsPerson = new Person("Charls Dunhig");
            _context.Attach(charlsPerson);

            var speakerRike = new Speaker("Rike Schmid");
            _context.Attach(speakerRike);

            var user = new User
            {
                EMail = "fabian.j@test.de",
                Username = "fabian",
                Password = "dasistzueinfach"
            };
            _context.Add(user);

            var book = new Book
            {
                ISBN = "12321321321",
                Title = "Denken hilft zwar, nützt aber nichts",
                Language = Language.German,
                Link = "https://google.de",
                Created = System.DateTime.Now,
                Auflage = "Erste",
                Creator = user
            };
            book.SetAuthors(new List<Author> { danAuthor });
            book.SetCategories(new List<Category> { categoryBoerse, categoryPsyche });
            _context.Add(book);
            user.AddNewBook(book);

            var book2 = new Book
            {
                ISBN = "345890438590345",
                Title = "Das Trügerische Gehirn",
                Language = Language.German,
                Link = "https://google.de",
                Created = System.DateTime.Now,
                Auflage = "Dritte",
                Creator = user
            };
            book2.SetAuthors(new List<Author> { juliaAuthor, hansAuthor });
            book2.SetCategories(new List<Category> { categoryGehirn });
            _context.Add(book2);
            user.AddNewBook(book2);
            user.AddBookRecommendation(book, book2, "Sie findet das Buch ganz toll");

            _context.SaveChanges();
            TestSeededData();
        }

        public void TestSeededData()
        {
            var authors = _context.Authors.ToList();
            Print<Author>(authors);
            var categories = _context.Categories.ToList();
            Print<Category>(categories);
            var people = _context.People.ToList();
            Print<Person>(people);
            var speaker = _context.Speakers.ToList();
            Print<Speaker>(speaker);
            var users = _context.Users.ToList();
            Print<User>(users);
            var books = _context.Books.ToList();
            Print<Book>(books);
        }

        public void Print<T>(List<T> data)
        {
            foreach (var item in data)
            {
                System.Console.WriteLine(item);
            }
        }
    }
}
