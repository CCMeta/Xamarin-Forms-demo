//using Dapper;
using Microsoft.AspNetCore.Authorization;
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
        private readonly int _uid;

        public PostsController(PostsRepository PostsRepository)
        {
            _PostsRepository = PostsRepository;
            if (HttpContext.Items.TryGetValue("uid", out var uid))
            {
                _uid = (int)uid;
                return;
            }
            throw new Exception("Controller Init _uid fucked");
        }

        // GET: api/<PostsController>
        [HttpGet]
        public async Task<IEnumerable<Posts>> GetAsync()
        {
            string page = HttpContext.Request.Query.TryGetValue("p", out var p) ? p.ToString() : "1";
            //string order = HttpContext.Request.Query.TryGetValue("o", out var o) ? o.ToString() : "1";
            return await _PostsRepository.GetList(_uid, page: Convert.ToInt32(page), limit: 5);
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
            post.uid = _uid;
            if (await _PostsRepository.Post(post) > 0)
                return Ok();
            return BadRequest();
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
