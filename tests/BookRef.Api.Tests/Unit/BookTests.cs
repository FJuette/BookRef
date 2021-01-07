using System;
using BookRef.Api.Exceptions;
using BookRef.Api.Models;
using BookRef.Api.Models.ValueObjects;
using FluentAssertions;
using Xunit;

namespace BookRef.Api.Tests.Unit
{
    public class BookTests
    {
        [Fact]
        public void AddCategory_Success_SingleItem()
        {
            //Given
            var book = new Book("123", "Example book", "test");
            var c = new Category("My Category");

            //When
            book.AddCategory(c, false);

            //Then
            book.BookCategories.Length().Should().Be(1);
        }

        [Fact]
        public void RemoveCategory_Success_EmptyList()
        {
            //Given
            var book = new Book("123", "Example book", "test");
            var c = new Category("My Category");

            //When
            book.AddCategory(c, false);
            book.RemoveCategory(c);

            //Then
            book.BookCategories.Length().Should().Be(0);
        }

        [Fact]
        public void AddCategory_CategoryAlreadyAdded_Exception()
        {
            //Given
            var book = new Book("123", "Example book", "test");
            var c = new Category("My Category");

            //When
            book.AddCategory(c, false);
            Action act = () => book.AddCategory(c, true);

            //Then
            act.Should().Throw<BookException>();
        }

        [Fact]
        public void AddCategory_SecondNotPrimary_Success()
        {
            //Given
            var book = new Book("123", "Example book", "test");
            var c = new Category("My Category");
            var c2 = new Category("Second Category");

            //When
            book.AddCategory(c, true);
            book.AddCategory(c2, false);

            //Then
            book.BookCategories.Length().Should().Be(2);
        }

        [Fact]
        public void AddCategory_SecondPrimary_Exception()
        {
            //Given
            var book = new Book("123", "Example book", "test");
            var c = new Category("My Category");
            var c2 = new Category("Second Category");

            //When
            book.AddCategory(c, true);
            Action act = () => book.AddCategory(c2, true);

            //Then
            act.Should().Throw<BookException>();
        }
    }
}
