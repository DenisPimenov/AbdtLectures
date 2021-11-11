using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Practice2.Models;

namespace Practice2.Controllers
{
    record ApiError(int Code, string Message);

    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private static readonly Dictionary<long, User> Users = new()
        {
            [1] = new User(1, "Owner", Role.Admin)
        };

        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(typeof(ApiError), 404)]
        public ActionResult<User> Get(long id)
        {
            if (Users.ContainsKey(id))
            {
                return Ok(Users[id]);
            }

            return NotFound(new ApiError(1, "User not found"));
        }
    }
}