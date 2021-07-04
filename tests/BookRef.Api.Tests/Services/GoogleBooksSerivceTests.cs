using BookRef.Api.Services;
using FluentAssertions;
using Xunit;

namespace BookRef.Api.Tests.Services
{
    public class GoogleBooksSerivceTests
    {

        [Fact]
        public void FindBook_CustomTest_BookFound()
        {
            // Arrange
            var srv = new GoogleBooksSerivce();

            // Act
            var book = srv.FindBook("9783936086355");

            // Assert
            book.IsSome.Should().BeTrue();
        }

        // Add tests for failing isbn
        [Fact]
        public void FindBook_ByIsbn_BookFound()
        {
            // Arrange
            var srv = new GoogleBooksSerivce();

            // Act
            var book = srv.FindBook("9783446260290");

            // Assert
            book.IsSome.Should().BeTrue();
            book.IfSome(x =>  x.Title.Should().Be("Böse"));
            book.IfSome(x =>  x.Identifier.Should().Be("9783446260290"));
        }

        [Theory]
        [InlineData("9783446260290")]
        [InlineData("9783662533253")]
        [InlineData("9783936086355")]
        public void FindBook_DifferentIsbn_BookFound(string isbn)
        {
            // Arrange
            var srv = new GoogleBooksSerivce();

            // Act
            var book = srv.FindBook(isbn);

            // Assert
            book.IsSome.Should().BeTrue();
            book.IfSome(x =>  x.Identifier.Should().Be(isbn));
        }

        [Fact]
        public void FindBook_IsbnFormat_BookFound()
        {
            // Arrange
            var srv = new GoogleBooksSerivce();

            // Act
            var book = srv.FindBook("978-3446260290");

            // Assert
            book.IsSome.Should().BeTrue();
            book.IfSome(x =>  x.Title.Should().Be("Böse"));
        }

        // Google is updating their database, book is no in the database
        // [Fact]
        // public void FindBook_CorrectIsbn_NoBookFound()
        // {
        //     // Arrange
        //     var srv = new GoogleBooksSerivce();

        //     // Act
        //     var book = srv.FindBook("9783442362851");

        //     // Assert
        //     book.IsSome.Should().BeFalse();
        // }
    }
}
