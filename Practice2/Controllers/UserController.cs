using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Practice2.Models;

namespace Practice2.Controllers
{
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

        /// <summary>
        /// Create new user
        /// </summary>
        /// <response code="200">User created</response>
        /// <response code="400">Invalid Role</response>
        /// <response code="409">User already exist</response>
        /// <response code="500">Oops! Can't add your user right now</response>
        /// <returns></returns>
        [HttpPost("")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(typeof(ApiError), 400)]
        [ProducesResponseType(typeof(ApiError), 409)]
        public ActionResult Post(User user)
        {
            if (!Enum.IsDefined(user.Role))
                return BadRequest(new ApiError(2, "Invalid Role"));
            if (Users.ContainsKey(user.Id))
                return Conflict(new ApiError(3, "User with same id already exist"));
            Users[user.Id] = user;
            return Ok();
        }

        /// <summary>
        /// Create new user
        /// </summary>
        /// <response code="200">User retrieved</response>
        /// <response code="400">User already exist</response>
        /// <response code="500">Oops! Can't add your user right now</response>
        /// <returns></returns>
        [Obsolete]
        [HttpPost("legacy")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(typeof(ApiError), 400)]
        public ActionResult Post(string name, Role role)
        {
            var userId = DateTime.UtcNow.Ticks;
            Users[userId] = new User(userId, name, role);
            return Ok();
        }
    }
}