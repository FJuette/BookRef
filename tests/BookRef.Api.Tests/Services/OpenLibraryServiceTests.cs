using BookRef.Api.Services;
using FluentAssertions;
using Xunit;

namespace BookRef.Api.Tests.Services
{
    public class OpenLibraryServiceTests
    {
        [Fact]
        public void FindBook_Success_ValidObject()
        {
            // Arrange
            var srv = new OpenLibraryService();

            // Act
            var book = srv.SearchByLibraryId("OL30777581M");

            // Assert
            book.Title.Should().Be("Tödliche Absicht");
        }

        // TODO Add tests for lib id not found

        [Fact]
        public void FindId_Success_ValidId()
        {
            // Arrange
            var srv = new OpenLibraryService();

            // Act
            var result = srv.GetLiraryId("9783442362851");

            // Assert
            result.Should().Be("OL30777581M");
        }

        // Add tests for failing isbn


        [Fact]
        public void FindBook_ByIsbn_BookFound()
        {
            // Arrange
            var srv = new OpenLibraryService();

            // Act
            var book = srv.FindBook("9783442362851");

            // Assert
            book.Title.Should().Be("Tödliche Absicht");
        }
    }
}
