using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin_Forms_demo_api.Models;

namespace Xamarin_Forms_demo_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : DefaultController
    {
        private readonly PostsRepository _PostsRepository;

        public PostsController(PostsRepository PostsRepository, IHttpContextAccessor context) : base(context)
        {
            _PostsRepository = PostsRepository;
        }

        // GET: api/<PostsController>
        [HttpGet]
        public async Task<IEnumerable<Posts>> GetAsync([FromQuery] int p)
        {
            return await _PostsRepository.GetList(_uid, maxId: p, limit: 5);
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
                return Ok(post);
            return BadRequest(post);
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
