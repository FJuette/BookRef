using System.Threading.Tasks;
using BookRef.Api.Recommandations.Queries;
using Microsoft.AspNetCore.Mvc;

namespace BookRef.Api.Controllers
{
    public class RecommendationController : BaseController
    {
        [HttpGet("api/recommendations/{id}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<BookRecViewModel>> GetAllBooks(long id) =>
            Ok(await Mediator.Send(new GetRecByBookQuery(id)));

    }
}
