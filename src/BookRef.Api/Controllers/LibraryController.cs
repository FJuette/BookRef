using System;
using System.Threading.Tasks;
using BookRef.Api.Library.Commands;
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
    }
}
