//using Dapper;
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
            string page = HttpContext.Request.Query.TryGetValue("p", out var p) ? p.ToString() : "1";
            //string order = HttpContext.Request.Query.TryGetValue("o", out var o) ? o.ToString() : "1";
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
        public async Task<IActionResult> PostAsync([FromBody] Posts post)
        {
            if (await _PostsRepository.Post(post) > 0)
                return Ok(post);
            return StatusCode(500);
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
