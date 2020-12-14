using System;
using System.Threading.Tasks;
using BookRef.Api.Users.Commands;
using BookRef.Api.Users.Queries;
using Microsoft.AspNetCore.Mvc;

namespace BookRef.Api.Controllers
{
    public class UserController : BaseController
    {
        [HttpGet("api/users")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<UsersViewModel>> GetAllUsers() =>
            Ok(await Mediator.Send(new GetAllUsersQuery()));

        [HttpPost("api/users")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Guid>> AddUser(
            [FromBody] CreateUserCommand command)
        {
            var id = await Mediator.Send(command);
            return CreatedAtAction(
                "GetAllUsers",
                id);
        }
    }
}
