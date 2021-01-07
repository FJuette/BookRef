using System;
using System.Linq;
using BookRef.Api.Exceptions;
using BookRef.Api.Models;
using BookRef.Api.Models.Relations;
using FluentAssertions;
using Xunit;

namespace BookRef.Api.Tests.Unit
{
    public class PersonalLibraryTests
    {
        public static PersonalLibrary GetEmptyLibrary()
        {
            return new PersonalLibrary(Guid.NewGuid(), new User("test", "test@test.de"));
        }

        [Fact]
        public void InternalLists_NewLibrary_AreEmtpy()
        {
            //Given
            var library = GetEmptyLibrary();

            //Then
            library.BookRecommedations.Should().BeEmpty();
            library.PersonRecommedations.Should().BeEmpty();
            library.MyBooks.Should().BeEmpty();
        }

        [Fact]
        public void User_NewLibrary_NotNull()
        {
            //Given
            var library = GetEmptyLibrary();

            //Then
            library.User.Should().NotBeNull();
        }

        [Fact]
        public void AddBookRecommendation_NewLibrary_SingleEntry()
        {
            //Given
            var library = GetEmptyLibrary();
            var sourceBook = new Book("123", "source book", "test");
            var targetBook = new Book("456", "target book", "test");

            //When
            library.AddNewBook(sourceBook, BookStatus.Active, null);
            library.AddBookRecommendation(sourceBook, targetBook);

            //Then
            library.BookRecommedations.Length().Should().Be(1);
        }

        [Fact]
        public void AddBookRecommendation_EmptyNote_NoteNotNull()
        {
            //Given
            var library = GetEmptyLibrary();
            var sourceBook = new Book("123", "source book", "test");
            var targetBook = new Book("456", "target book", "test");

            //When
            library.AddNewBook(sourceBook, BookStatus.Active, null);
            library.AddBookRecommendation(sourceBook, targetBook);

            //Then
            library.BookRecommedations.First().Note.Should().NotBeNull();
        }

        [Fact]
        public void AddBookRecommendation_BookNotInLibrary_Exception()
        {
            //Given
            var library = GetEmptyLibrary();
            var sourceBook = new Book("123", "source book", "test");
            var targetBook = new Book("456", "target book", "test");

            //When
            Action act = () => library.AddBookRecommendation(sourceBook, targetBook);

            //Then
            act.Should().Throw<LibraryException>();
        }

        [Fact]
        public void AddPersonRecommendation_NewLibrary_SingleEntry()
        {
            //Given
            var library = GetEmptyLibrary();
            var sourceBook = new Book("123", "source book", "test");
            var targetPerson = new Models.ValueObjects.Person("Tom Target");

            //When
            library.AddNewBook(sourceBook, BookStatus.Active, null);
            library.AddPersonRecommendation(sourceBook, targetPerson);

            //Then
            library.PersonRecommedations.Length().Should().Be(1);
        }

        [Fact]
        public void AddPersonRecommendation_EmptyNote_NoteNotNull()
        {
            //Given
            var library = GetEmptyLibrary();
            var sourceBook = new Book("123", "source book", "test");
            var targetPerson = new Models.ValueObjects.Person("Tom Target");

            //When
            library.AddNewBook(sourceBook, BookStatus.Active, null);
            library.AddPersonRecommendation(sourceBook, targetPerson);

            //Then
            library.PersonRecommedations.First().Note.Should().NotBeNull();
        }

        [Fact]
        public void AddPersonRecommendation_BookNotInLibrary_Exception()
        {
            //Given
            var library = GetEmptyLibrary();
            var sourceBook = new Book("123", "source book", "test");
            var targetPerson = new Models.ValueObjects.Person("Tom Target");

            //When
            Action act = () => library.AddPersonRecommendation(sourceBook, targetPerson);

            //Then
            act.Should().Throw<LibraryException>();
        }

        [Fact]
        public void ChangeBookStatus_WishToActive_StatusSet()
        {
            //Given
            var library = GetEmptyLibrary();
            var book = new Book("123", "Example book", "test");

            //When
            library.AddNewBook(book, BookStatus.Wish, "");
            var pb = library.MyBooks.First();
            pb.Status.Should().Be(BookStatus.Wish);
            library.ChangeBookStatus(pb.Id, BookStatus.Active);

            //Then
            pb.Status.Should().Be(BookStatus.Active);
        }

        [Fact]
        public void AddNewBook_EmptyLibrary_SingleEntry()
        {
            //Given
            var library = GetEmptyLibrary();
            var book = new Book("123", "Example book", "test");

            //When
            library.AddNewBook(book, BookStatus.Active, "");

            //Then
            library.MyBooks.Length().Should().Be(1);
        }

        [Fact]
        public void AddNewBook_BookAlreadyAdded_ThrowsException()
        {
            //Given
            var library = GetEmptyLibrary();
            var book = new Book("123", "Example book", "test");

            //When
            library.AddNewBook(book, BookStatus.Active, "");
            Action act = () => library.AddNewBook(book, BookStatus.Wish, "");

            //Then
            act.Should().Throw<LibraryException>();
        }

        [Fact]
        public void AddNewBook_AddingDifferntBooks_ListFilled()
        {
            //Given
            var library = GetEmptyLibrary();
            var book = new Book("123", "Example book", "test");
            var book2 = new Book("456", "Example book", "test");
            var book3 = new Book("123", "Same identifier, other name", "test");

            //When
            library.AddNewBook(book, BookStatus.Active, "");
            library.AddNewBook(book2, BookStatus.Active, "");
            library.AddNewBook(book3, BookStatus.Active, "");

            //Then
            library.MyBooks.Length().Should().Be(3);
        }
    }
}
