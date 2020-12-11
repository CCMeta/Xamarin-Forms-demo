﻿//using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin_Forms_demo_api.Models;

namespace Xamarin_Forms_demo_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly PostsRepository _PostsRepository;

        public PostsController(PostsRepository PostsRepository)
        {
            _PostsRepository = PostsRepository;
        }

        // GET: api/<PostsController>
        [HttpGet]
        public async Task<IEnumerable<Posts>> GetAsync()
        {
            string page = HttpContext.Request.Query.TryGetValue("p", out var StringValues) ? StringValues.ToString() : "1";
            return await _PostsRepository.GetList(page: Convert.ToInt32(page), limit: 5);
        }

        // GET api/<PostsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<PostsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<PostsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PostsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
