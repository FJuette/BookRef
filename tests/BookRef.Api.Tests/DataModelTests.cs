using System.Threading.Tasks;
using BookRef.Api.Models.ValueObjects;
using BookRef.Api.Persistence;
using BookRef.Api.Tests.Common;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BookRef.Api.Tests
{
    public class DataModelTests
        : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly BookRefDbContext _context;

        public DataModelTests(
            CustomWebApplicationFactory<Startup> factory) =>
            _context = (BookRefDbContext)factory.Services.GetService(typeof(BookRefDbContext));

        [Fact]
        public async Task Context_IsAbleToQuery_NoExceptions()
        {
            // Act
            var authors = await _context.Authors.ToListAsync();
            var categories = await _context.Categories.ToListAsync();
            var people = await _context.People.ToListAsync();
            var speakers = await _context.Speakers.ToListAsync();
            var books = await _context.Books.ToListAsync();
            var libraries = await _context.Libraries.ToListAsync();
            var bookRecommendations = await _context.BookRecommedations.ToListAsync();

            // Assert
            authors.Count.Should().BeGreaterOrEqualTo(0);
            categories.Count.Should().BeGreaterOrEqualTo(0);
            people.Count.Should().BeGreaterOrEqualTo(0);
            speakers.Count.Should().BeGreaterOrEqualTo(0);
            books.Count.Should().BeGreaterOrEqualTo(0);
            libraries.Count.Should().BeGreaterOrEqualTo(0);
            bookRecommendations.Count.Should().BeGreaterOrEqualTo(0);
        }

        [Fact]
        public async Task Context_CountIsGreaterZero_HasCount()
        {
            // Act
            var authors = await _context.Authors.ToListAsync();
            var categories = await _context.Categories.ToListAsync();
            var people = await _context.People.ToListAsync();
            var speakers = await _context.Speakers.ToListAsync();
            var books = await _context.Books.ToListAsync();
            var libraries = await _context.Libraries.ToListAsync();
            var bookRecommendations = await _context.BookRecommedations.ToListAsync();

            // Assert
            authors.Count.Should().BeGreaterOrEqualTo(1);
            categories.Count.Should().BeGreaterOrEqualTo(1);
            people.Count.Should().BeGreaterOrEqualTo(1);
            speakers.Count.Should().BeGreaterOrEqualTo(1);
            books.Count.Should().BeGreaterOrEqualTo(1);
            libraries.Count.Should().BeGreaterOrEqualTo(1);
            bookRecommendations.Count.Should().BeGreaterOrEqualTo(1);
        }
    }
}
