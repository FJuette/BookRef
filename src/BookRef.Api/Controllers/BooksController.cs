using System.Threading.Tasks;
using BookRef.Api.Authors.Commands;
using BookRef.Api.Authors.Queries;
using BookRef.Api.Books.Queries;
using BookRef.Api.Categories.Queries;
using BookRef.Api.People.Queries;
using BookRef.Api.Speakers.Queries;
using Microsoft.AspNetCore.Mvc;

namespace BookRef.Api.Controllers
{
    public class BooksController : BaseController
    {
        #region Queries

        [HttpGet("api/books")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<BooksViewModel>> GetAllBooks() =>
            Ok(await Mediator.Send(new GetAllBooksQuery()));

        [HttpGet("api/books/active")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ActiveBooksViewModel>> GetAllActiveBooks() =>
            Ok(await Mediator.Send(new GetAllActiveBooksQuery()));

        [HttpGet("api/books/wishlist")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<WishlistViewModel>> GetWishlist() =>
            Ok(await Mediator.Send(new GetWishlistQuery()));

        [HttpGet("api/books/done")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<DoneViewModel>> GetDone() =>
            Ok(await Mediator.Send(new GetDoneQuery()));

        [HttpGet("api/authors")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<AuthorsViewModel>> GetAllAuthors() =>
            Ok(await Mediator.Send(new GetAllAuthorsQuery()));

        [HttpGet("api/categories")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<CategoriesViewModel>> GetAllCategories() =>
            Ok(await Mediator.Send(new GetAllCategoriesQuery()));

        [HttpGet("api/people")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<PeopleViewModel>> GetAllPeople() =>
            Ok(await Mediator.Send(new GetAllPeopleQuery()));

        [HttpGet("api/speaker")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<SpeakersViewModel>> GetAllSpeaker() =>
            Ok(await Mediator.Send(new GetAllSpeakerQuery()));

        #endregion

        #region Commands

        [HttpPost("api/authors")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<long>> AddAuthor(
            [FromBody] AddAuthorCommand command)
        {
            var id = await Mediator.Send(command);
            return CreatedAtAction(
                "GetAllAuthors",
                id);
        }
        #endregion
    }
}
