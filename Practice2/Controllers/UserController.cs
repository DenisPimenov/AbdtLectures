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

        [HttpGet("{id:long}")]
        public ActionResult<User> Get(long id)
        {
            if (Users.ContainsKey(id))
            {
                return Ok(Users[id]);
            }

            return NotFound();
        }
    }
}