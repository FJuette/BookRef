using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BookRef.Api.Models;
using BookRef.Api.Models.Relations;
using BookRef.Api.Models.ValueObjects;
using BookRef.Api.Persistence;
using BookRef.Api.Services;

namespace BookRef.Api.Extensions
{
    public class SampleDataSeeder
    {
        private readonly BookRefDbContext _context;
        private readonly IBookApiService _service;

        //private readonly AggregateRepository _repository;

        public SampleDataSeeder(
            BookRefDbContext context,
            IBookApiService service)
            {
                _context = context;
            _service = service;
            //_repository = repository;
        }

        public async Task SeedAll()
        {
            var danAuthor = new Author("Dan Ariely");
            _context.Authors.Add(danAuthor);
            var hansAuthor = new Author("Hans Rosling");
            _context.Authors.Add(hansAuthor);
            var juliaAuthor = new Author("Julia Shaw");
            _context.Authors.Add(juliaAuthor);
            var zimbardoAuthor = new Author("Philip Zimbardo");
            _context.Authors.Add(zimbardoAuthor);
            var freudAuthor = new Author("Sigmund Freud");
            _context.Authors.Add(freudAuthor);
            var bernaysAuthor = new Author("Ulrich Preis");
            _context.Authors.Add(bernaysAuthor);

            var categories = File.ReadAllLines("Categories.txt").Select(e => new Category(e));
            _context.Categories.AddRange(categories);

            var categoryGehirn = categories.First(e => e.Name == "Bildung");
            var categoryPsyche = categories.First(e => e.Name == "Persönliche Entwicklung");
            var categoryBoerse = categories.First(e => e.Name == "Persönliche Finanzen");

            var charlsPerson = new Person("Charls Dunhig");
            _context.People.Add(charlsPerson);

            var speakerRike = new Speaker("Rike Schmid");
            _context.Speakers.Add(speakerRike);

            var user = new User("admin", "fabian.j@test.de");
            user.SetPassword("geheim");
            _context.Add(user);
            //_context.SaveChanges();

            // var libraryId = Guid.NewGuid();
            // var esStream = await _repository.LoadAsync<PersonalLibrary>(libraryId);
            // esStream.Create(libraryId, user.Id);
            // await _repository.SaveAsync(esStream);

            // var library = await _repository.LoadAsync<PersonalLibrary>(libraryId);
            var library = new PersonalLibrary(new Guid("EE471115-0425-489B-931A-8B3F7F187205"), user);
            _context.Libraries.Add(library);
            _context.SaveChanges();

            var book = new Book("9783426300886", "Denken hilft zwar, nützt aber nichts", "admin")
            {
                Language = BookLanguage.German,
                Link = "https://www.amazon.de/Denken-hilft-zwar-n%C3%BCtzt-nichts/dp/3426300885",
                Created = new System.DateTime(2020, 05, 11),
                Auflage = "Sechste",
                Subtitle = "Warum wir immer wieder unvernünftige Entscheidungen"
            };
            book.SetAuthors(new List<Author> { danAuthor });
            book.SetCategories(new List<Category> { categoryBoerse, categoryPsyche });
            library.AddNewBook(book, BookStatus.Active, "");
            _context.Books.Add(book);

            var book2 = new Book("978-3453604483", "Das trügerische Gedächtnis", "admin")
            {
                Language = BookLanguage.German,
                Link = "https://www.amazon.de/Das-tr%C3%BCgerische-Ged%C3%A4chtnis-Erinnerungen-f%C3%A4lscht/dp/3453604482",
                Created = new System.DateTime(2020, 06, 12),
                Auflage = "Erste",
                Subtitle = "Wie unser Gehirn Erinnerungen fälscht"
            };
            book2.SetAuthors(new List<Author> { juliaAuthor, hansAuthor });
            book2.SetCategories(new List<Category> { categoryGehirn });
            _context.Add(book2);
            library.AddNewBook(book2, BookStatus.Active, "");
            library.AddBookRecommendation(book, book2, "Sie findet das Buch ganz toll");
            library.AddPersonRecommendation(book, charlsPerson, "Seine arbeiten zum Thema 'Habits' sind interessant");

            var book3 = new Book("978-3446260290", "Böse", "admin")
            {
                Language = BookLanguage.German,
                Link = "https://www.amazon.de/B%C3%B6se-Die-Psychologie-unserer-Abgr%C3%BCnde/dp/3446260293",
                Created = new System.DateTime(2020, 07, 15),
                Auflage = "Dritte",
                Subtitle = "Die Psychologie unserer Abgründe"
            };
            book3.SetAuthors(new List<Author> { juliaAuthor });
            book3.SetCategories(new List<Category> { categoryGehirn });
            _context.Add(book3);
            library.AddNewBook(book3, BookStatus.Active, "");

            var book4 = new Book("978-3662533253", "Der Luzifer-Effekt", "admin")
            {
                Language = BookLanguage.German,
                Link = "https://www.amazon.de/B%C3%B6se-Die-Psychologie-unserer-Abgr%C3%BCnde/dp/3446260293",
                Created = new System.DateTime(2020, 07, 18),
                Auflage = "Erste",
                Subtitle = "Die Macht der Umstände und die Psychologie des Bösen"
            };
            book4.SetAuthors(new List<Author> { zimbardoAuthor });
            book4.SetCategories(new List<Category> { categoryGehirn });
            _context.Add(book4);
            library.AddNewBook(book4, BookStatus.Active, "");

            var book5 = new Book("978-3730604540", "Massenpsychologie und Ich-Analyse", "admin")
            {
                Language = BookLanguage.English,
                Link = "https://www.amazon.de/Massenpsychologie-Ich-Analyse-Sigmund-Freud/dp/3730604546",
                Created = new System.DateTime(2020, 08, 20),
                Auflage = "Erste",
                //Creator = user
            };
            book5.SetAuthors(new List<Author> { freudAuthor });
            book5.SetCategories(new List<Category> { categoryPsyche });
            _context.Add(book5);
            library.AddNewBook(book5, BookStatus.Active, "");

            var book6 = new Book("978-3936086355", "Propaganda", "admin")
            {
                Language = BookLanguage.English,
                Link = "https://www.amazon.de/Propaganda-Die-Kunst-Public-Relations/dp/3936086354",
                Created = new System.DateTime(2020, 09, 1),
                Auflage = "Dritte",
                Subtitle = "Die Kunst der Public Relations"
            };
            book6.SetAuthors(new List<Author> { bernaysAuthor });
            book6.SetCategories(new List<Category> { categoryGehirn });
            _context.Add(book6);
            library.AddNewBook(book6, BookStatus.Active, "ffffff");

            _context.SaveChanges();
            var authors = _context.Authors.ToList();
            var book7 = _service.FindBook("9783813504897");
            book7 = new BookService().MatchAuthors(authors, book7);

            book7.IfSome(e =>
            {
                _context.Add(e);
                library.AddNewBook(e, BookStatus.Active, "admin");
            });
            _context.SaveChanges();
            var book8 = _service.FindBook("9783406757006");
            book8 = new BookService().MatchAuthors(authors, book8);

            book8.IfSome(e =>
            {
                _context.Add(e);
                library.AddNewBook(e, BookStatus.Active, "admin");
            });


            // _context.Libraries.Add(library);
            //await _repository.SaveAsync(library);
            _context.SaveChanges();




            // var id = Guid.NewGuid();

            // _context.SaveChanges();

            // var library = await _repository.LoadAsync<PersonalLibrary>(id);
            // // TODO put real userID here
            // library.Create(id, 1);
            // await _repository.SaveAsync(library);

            // var library2 = await _repository.LoadAsync<PersonalLibrary>(library.Id);


            // library2.AddNewBook(book);



            // _context.Add(library2);
            // await _repository.SaveAsync(library2);
            // _context.SaveChanges();
            // TestSeededData();
        }

        public void TestSeededData()
        {
            // var authors = _context.Authors.ToList();
            // Print<Author>(authors);
            // var categories = _context.Categories.ToList();
            // Print<Category>(categories);
            // var people = _context.People.ToList();
            // Print<Person>(people);
            // var speaker = _context.Speakers.ToList();
            // Print<Speaker>(speaker);
            // var users = _context.Users.ToList();
            // Print<User>(users);
            // var books = _context.Books.ToList();
            // Print<Book>(books);
            // var library = _context.Libraries.ToList();
            // Print<PersonalLibrary>(library);
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
