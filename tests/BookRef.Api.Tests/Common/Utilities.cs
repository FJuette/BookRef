using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BookRef.Api.Models;
using BookRef.Api.Persistence;

namespace BookRef.Api.Tests.Common
{
    public class Utilities
    {
        public static StringContent GetRequestContent(
            object obj) =>
            new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");

        public static async Task<T> GetResponseContent<T>(
            HttpResponseMessage response)
        {
            var stringResponse = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<T>(stringResponse);

            return result;
        }

        public static void InitializeDbForTests(
            BookRefDbContext context)
        {
            context.Stories.Add(
                new UserStory(
                    "My demo user story",
                    Priority.Create(1).Value,
                    "Info",
                    "Provide long text here",
                    BusinessValue.BV900));
            context.SaveChanges();
        }
    }
}
