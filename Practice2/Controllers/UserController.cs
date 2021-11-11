using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Practice2.Models;

namespace Practice2.Controllers
{
    /// <summary>
    /// Api error
    /// </summary>
    record ApiError(int Code, string Message);

    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private static readonly Dictionary<long, User> Users = new()
        {
            [1] = new User(1, "Owner", Role.Admin)
        };

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="id">User id</param>
        /// <response code="200">User retrieved</response>
        /// <response code="404">User not found</response>
        /// <response code="500">Oops! Can't lookup your user right now</response>
        /// <returns></returns>
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