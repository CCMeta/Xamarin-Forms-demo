using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin_Forms_demo_api.Models;

namespace Xamarin_Forms_demo_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : DefaultController
    {
        private readonly ChatsRepository _chatsRepository;

        public ChatsController(ChatsRepository chatsRepository, IHttpContextAccessor context) : base(context)
        {
            _chatsRepository = chatsRepository;
        }

        // GET: api/<ChatsController> 在线的时候websocketPush 那么没在线的留言怎么办呢 一开始连结WS后主动拉取吗？
        [HttpGet]
        public async Task<ActionResult> GetAsync([FromQuery] int partner_id)
        {
            var result = await _chatsRepository.GetList(_uid, partner_id);
            return Ok(result);
        }

        // GET api/<ChatsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ChatsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ChatsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ChatsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
