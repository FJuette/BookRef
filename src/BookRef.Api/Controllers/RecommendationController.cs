using System.Threading.Tasks;
using BookRef.Api.Recommandations.Queries;
using Microsoft.AspNetCore.Mvc;

namespace BookRef.Api.Controllers
{
    public class RecommendationController : BaseController
    {
        [HttpGet("api/recommendations/{bookId}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<BookRecViewModel>> GetAllBookRec(long bookId) =>
            Ok(await Mediator.Send(new GetRecByBookQuery(bookId)));


        // [HttpGet("api/recommendations")]
        // [ProducesResponseType(200)]
        // public async Task<ActionResult<RecsViewModel>> GetAllBooks(long id) =>
        //     Ok(await Mediator.Send(new GetRecByBookQuery(id)));

    }
}
