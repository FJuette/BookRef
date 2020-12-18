using System;
using System.Threading.Tasks;
using BookRef.Api.Books.Queries;
using BookRef.Api.Library.Commands;
using BookRef.Api.Library.Queries;
using BookRef.Api.Models;
using BookRef.Api.Users.Commands;
using BookRef.Api.Users.Queries;
using Microsoft.AspNetCore.Mvc;

namespace BookRef.Api.Controllers
{
    public class LibraryController : BaseController
    {
        [HttpPost("api/library")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Guid>> AddLibrary(
            [FromBody] CreateLibraryCommand command)
        {
            var id = await Mediator.Send(command);
            return Created("TODO", null);
        }

        [HttpPost("api/library/add-book")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<bool>> AddBookToLibrary(
            [FromBody] AddBookToLibraryCommand command)
        {
            var id = await Mediator.Send(command);
            return Created("TODO", null);
        }

        [HttpGet("api/library")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<PersonalLibrary>> GetLibrary() =>
            Ok(await Mediator.Send(new GetLibraryFromESQuery()));
    }
}
