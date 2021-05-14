using System.Collections.Generic;
using System.Linq;
using BookRef.Api.Models;
using LanguageExt;
using RestSharp;
using System.Text.RegularExpressions;
using BookRef.Api.Models.ValueObjects;

namespace BookRef.Api.Services
{

    // Example https://www.googleapis.com/books/v1/volumes?q=isbn:9783446260290
    public class GoogleBooksSerivce : IBookApiService
    {
        public Option<Book> FindBook(string isbn)
        {
            var client = new RestClient("https://www.googleapis.com/books/v1");
            client.ThrowOnAnyError = true;

            var request = new RestRequest($"volumes?q=isbn:{ParseIsbn(isbn)}", DataFormat.Json);

            var response = client.Get<GApiResponse>(request).Data;

            return GetBook(response);
        }

        private string ParseIsbn(string input) => Regex.Replace(input, @"\D", "");

        private Option<Book> GetBook(GApiResponse data) => data.TotalItems switch
        {
            < 1 => null,
            > 1 => throw new System.Exception("Multiple books found for ISBN"),
            1 => InitBook(data.Items.First()),
        };

        private Book InitBook(GApiItem item)
        {
            var isbn = item.VolumeInfo.IndustryIdentifiers.First(e => e.Type == "ISBN_13").Identifier;

            var book = new Book(isbn, item.VolumeInfo.Title, "googleapis.com")
            {
                Subtitle = item.VolumeInfo.Subtitle,
            }
            .SetAdditionalApiData(item.Etag, item.SelfLink, item.SearchInfo?.TextSnippet)
            .SetAuthors(item.VolumeInfo.Authors.Select(e => new Author(e)));

            return book;
        }


        private record GApiResponse
        {
            public string Kind { get; init; }
            public int TotalItems { get; init; }
            public List<GApiItem> Items { get; init; }

        }
        private record GApiItem
        {
            public string Id { get; init; }
            public string Etag { get; init; }
            public string SelfLink { get; init; }
            public GApiVolumeInfo VolumeInfo { get; init; }
            public GApisSearchInfo SearchInfo { get; init; }
        }

        private record GApiVolumeInfo
        {
            public string Title { get; init; }
            public string? Subtitle { get; init; }
            public List<string> Authors { get; init; }
            public List<GApiIndustryIdentifiers> IndustryIdentifiers { get; init; }
        }

        private record GApisSearchInfo
        {
            public string? TextSnippet { get; init; }
        }

        private record GApiIndustryIdentifiers
        {
            public string Type { get; init; }
            public string Identifier { get; init; }
        }
    }
}
