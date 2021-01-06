using System;
using System.Net.Http;
using BookRef.Api.Models;
using RestSharp;

namespace BookRef.Api.Services
{
    public interface IOpenLibraryService
    {
        Book FindBook(string isbn);
        string GetLiraryId(string isbn);
        Book SearchByLibraryId(string id);
    }

    public class OpenLibraryService : IOpenLibraryService
    {
        public Book FindBook(string isbn)
        {
            return GetBook(GetLiraryId(isbn));
        }

        public string GetLiraryId(string isbn)
        {
            var handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            using var client = new HttpClient(handler);
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri($"https://openlibrary.org/isbn/{isbn}"),
                Method = HttpMethod.Get
            };

            HttpResponseMessage response = client.Send(request);
            var redirectUri = response.Headers.Location;
            return redirectUri.Segments[redirectUri.Segments.Length - 1];
        }

        public Book SearchByLibraryId(string id)
        {
            return GetBook(id);
        }

        private Book GetBook(string id)
        {
            var client = new RestClient("https://openlibrary.org");
            var request = new RestRequest($"books/{id}.json", DataFormat.Json);
            var response = client.Get<OpenLibBookDto>(request);
            var book = new Book("", response.Data.Title, "openlibrary.org")
            {
                Subtitle = response.Data.Subtitle
            };
            return book;
        }

        public class OpenLibBookDto
        {
            public string Title { get; set; }
            public string Subtitle { get; set; }
        }
    }
}
