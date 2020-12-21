using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookRef.Api.Models;
using BookRef.Api.Models.Relations;
using BookRef.Api.Models.ValueObjects;
using BookRef.Api.Persistence;

namespace BookRef.Api.Extensions
{
    public class SampleDataSeeder
    {
        private readonly BookRefDbContext _context;
        private readonly AggregateRepository _repository;

        public SampleDataSeeder(
            BookRefDbContext context, AggregateRepository repository)
            {
                _context = context;
                _repository = repository;
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
            var bernaysAuthor = new Author("Edward Bernays");
            _context.Authors.Add(bernaysAuthor);

            var categoryGehirn = new Category("Gehirn");
            _context.Categories.Add(categoryGehirn);
            var categoryPsyche = new Category("Psyche");
            _context.Categories.Add(categoryPsyche);
            var categoryBoerse = new Category("Börse");
            _context.Categories.Add(categoryBoerse);

            var charlsPerson = new Person("Charls Dunhig");
            _context.People.Add(charlsPerson);

            var speakerRike = new Speaker("Rike Schmid");
            _context.Speakers.Add(speakerRike);

            var user = new User
            {
                EMail = "fabian.j@test.de",
                Username = "Admin",
                Password = "dasistzueinfach"
            };
            _context.Add(user);

            var library = new PersonalLibrary(user);

            var book = new Book("978-3426300886", "Denken hilft zwar, nützt aber nichts: Warum wir immer wieder unvernünftige Entscheidungen")
            {
                Language = BookLanguage.German,
                Link = "https://www.amazon.de/Denken-hilft-zwar-n%C3%BCtzt-nichts/dp/3426300885",
                Created = new System.DateTime(2020, 05, 11),
                Auflage = "Sechste",
                //Creator = user
            };
            book.SetAuthors(new List<Author> { danAuthor });
            book.SetCategories(new List<Category> { categoryBoerse, categoryPsyche });
            _context.Books.Add(book);

            var book2 = new Book("978-3453604483", "Das trügerische Gedächtnis: Wie unser Gehirn Erinnerungen fälscht")
            {
                Language = BookLanguage.German,
                Link = "https://www.amazon.de/Das-tr%C3%BCgerische-Ged%C3%A4chtnis-Erinnerungen-f%C3%A4lscht/dp/3453604482",
                Created = new System.DateTime(2020, 06, 12),
                Auflage = "Erste",
                //Creator = user
            };
            book2.SetAuthors(new List<Author> { juliaAuthor, hansAuthor });
            book2.SetCategories(new List<Category> { categoryGehirn });
            _context.Add(book2);
            library.AddBookDataSeeder(book2);
            library.AddBookRecommendation(book, book2, "Sie findet das Buch ganz toll");
            library.AddPersonRecommendation(book, charlsPerson, "Seine arbeiten zum Thema 'Habits' sind interessant");

            var book3 = new Book("978-3446260290", "Böse: Die Psychologie unserer Abgründe")
            {
                Language = BookLanguage.German,
                Link = "https://www.amazon.de/B%C3%B6se-Die-Psychologie-unserer-Abgr%C3%BCnde/dp/3446260293",
                Created = new System.DateTime(2020, 07, 15),
                Auflage = "Dritte",
                //Creator = user
            };
            book3.SetAuthors(new List<Author> { juliaAuthor });
            book3.SetCategories(new List<Category> { categoryGehirn });
            _context.Add(book3);
            //library2.AddNewBook(book3);

            var book4 = new Book("978-3662533253", "Der Luzifer-Effekt: Die Macht der Umstände und die Psychologie des Bösen")
            {
                Language = BookLanguage.German,
                Link = "https://www.amazon.de/B%C3%B6se-Die-Psychologie-unserer-Abgr%C3%BCnde/dp/3446260293",
                Created = new System.DateTime(2020, 07, 18),
                Auflage = "Erste",
                //Creator = user
            };
            book4.SetAuthors(new List<Author> { zimbardoAuthor });
            book4.SetCategories(new List<Category> { categoryGehirn });
            _context.Add(book4);
            //library2.AddNewBook(book4);

            var book5 = new Book("978-3730604540", "Massenpsychologie und Ich-Analyse")
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
            //library2.AddNewBook(book5);

            var book6 = new Book("978-3936086355", "Propaganda: Die Kunst der Public Relations")
            {
                Language = BookLanguage.English,
                Link = "https://www.amazon.de/Propaganda-Die-Kunst-Public-Relations/dp/3936086354",
                Created = new System.DateTime(2020, 09, 1),
                Auflage = "Dritte",
                //Creator = user
            };
            book6.SetAuthors(new List<Author> { bernaysAuthor });
            book6.SetCategories(new List<Category> { categoryGehirn });
            _context.Add(book6);
            //library2.AddNewBook(book6);


            _context.Libraries.Add(library);

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
