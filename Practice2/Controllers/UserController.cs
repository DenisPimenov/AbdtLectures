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
        private static readonly Dictionary<long, User> Users = new();

        [HttpGet("{id:long}")]
        public ActionResult Get(long id)
        {
            throw new NotImplementedException();
        }
    }
}