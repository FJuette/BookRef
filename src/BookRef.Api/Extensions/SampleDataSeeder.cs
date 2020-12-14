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
            var danAuthor = new Author("Dan Ariely");
            _context.Attach(danAuthor);
            var hansAuthor = new Author("Hans Rosling");
            _context.Attach(hansAuthor);
            var juliaAuthor = new Author("Julia Shaw");
            _context.Attach(juliaAuthor);
            var zimbardoAuthor = new Author("Philip Zimbardo");
            _context.Attach(zimbardoAuthor);
            var freudAuthor = new Author("Sigmund Freud");
            _context.Attach(freudAuthor);
            var bernaysAuthor = new Author("Edward Bernays");
            _context.Attach(bernaysAuthor);

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
                Username = "Admin",
                Password = "dasistzueinfach",
                Id = new System.Guid()
            };

            var book = new Book
            {
                ISBN = "978-3426300886",
                Title = "Denken hilft zwar, nützt aber nichts: Warum wir immer wieder unvernünftige Entscheidungen",
                Language = Language.German,
                Link = "https://www.amazon.de/Denken-hilft-zwar-n%C3%BCtzt-nichts/dp/3426300885",
                Created = new System.DateTime(2020, 05, 11),
                Auflage = "Sechste",
                Creator = user
            };
            book.SetAuthors(new List<Author> { danAuthor });
            book.SetCategories(new List<Category> { categoryBoerse, categoryPsyche });
            _context.Add(book);
            user.AddNewBook(book);

            var book2 = new Book
            {
                ISBN = "978-3453604483",
                Title = "Das trügerische Gedächtnis: Wie unser Gehirn Erinnerungen fälscht",
                Language = Language.German,
                Link = "https://www.amazon.de/Das-tr%C3%BCgerische-Ged%C3%A4chtnis-Erinnerungen-f%C3%A4lscht/dp/3453604482",
                Created = new System.DateTime(2020, 06, 12),
                Auflage = "Erste",
                Creator = user
            };
            book2.SetAuthors(new List<Author> { juliaAuthor, hansAuthor });
            book2.SetCategories(new List<Category> { categoryGehirn });
            _context.Add(book2);
            user.AddNewBook(book2);
            user.AddBookRecommendation(book, book2, "Sie findet das Buch ganz toll");
            user.AddPersonRecommendation(book, charlsPerson, "Seine arbeiten zum Thema 'Habits' sind interessant");

            var book3 = new Book
            {
                ISBN = "978-3446260290",
                Title = "Böse: Die Psychologie unserer Abgründe",
                Language = Language.German,
                Link = "https://www.amazon.de/B%C3%B6se-Die-Psychologie-unserer-Abgr%C3%BCnde/dp/3446260293",
                Created = new System.DateTime(2020, 07, 15),
                Auflage = "Dritte",
                Creator = user
            };
            book3.SetAuthors(new List<Author> { juliaAuthor });
            book3.SetCategories(new List<Category> { categoryGehirn });
            _context.Add(book3);
            user.AddNewBook(book3);

            var book4 = new Book
            {
                ISBN = "978-3662533253",
                Title = "Der Luzifer-Effekt: Die Macht der Umstände und die Psychologie des Bösen",
                Language = Language.German,
                Link = "https://www.amazon.de/B%C3%B6se-Die-Psychologie-unserer-Abgr%C3%BCnde/dp/3446260293",
                Created = new System.DateTime(2020, 07, 18),
                Auflage = "Erste",
                Creator = user
            };
            book4.SetAuthors(new List<Author> { zimbardoAuthor });
            book4.SetCategories(new List<Category> { categoryGehirn });
            _context.Add(book4);
            user.AddNewBook(book4);

            var book5 = new Book
            {
                ISBN = "978-3730604540",
                Title = "Massenpsychologie und Ich-Analyse",
                Language = Language.German,
                Link = "https://www.amazon.de/Massenpsychologie-Ich-Analyse-Sigmund-Freud/dp/3730604546",
                Created = new System.DateTime(2020, 08, 20),
                Auflage = "Erste",
                Creator = user
            };
            book5.SetAuthors(new List<Author> { freudAuthor });
            book5.SetCategories(new List<Category> { categoryGehirn });
            _context.Add(book5);
            user.AddNewBook(book5, BookStatus.Wish);

            var book6 = new Book
            {
                ISBN = "978-3936086355",
                Title = "Propaganda: Die Kunst der Public Relations",
                Language = Language.German,
                Link = "https://www.amazon.de/Propaganda-Die-Kunst-Public-Relations/dp/3936086354",
                Created = new System.DateTime(2020, 09, 1),
                Auflage = "Dritte",
                Creator = user
            };
            book6.SetAuthors(new List<Author> { bernaysAuthor });
            book6.SetCategories(new List<Category> { categoryGehirn });
            _context.Add(book6);
            user.AddNewBook(book6, BookStatus.Done);

            _context.Add(user);

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
