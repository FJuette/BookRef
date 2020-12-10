using System.Threading.Tasks;
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
        [HttpGet("api/books")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<BooksViewModel>> GetAllBooks() =>
            Ok(await Mediator.Send(new GetAllBooksQuery()));

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
    }
}
