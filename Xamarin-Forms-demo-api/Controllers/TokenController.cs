using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin_Forms_demo_api.Models;
using Xamarin_Forms_demo_api.Services;

namespace Xamarin_Forms_demo_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly UsersRepository _usersRepository;
        private readonly SessionService _sessionService;

        public TokenController(UsersRepository usersRepository, SessionService sessionService)
        {
            _usersRepository = usersRepository;
            _sessionService = sessionService;
        }
        // GET: api/<TokenController>
        [HttpGet]
        public IEnumerable<string> Get([FromHeader] string TOKEN)
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<TokenController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<TokenController>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Users user)
        {
            var result = await _usersRepository.Get(user);
            if (result.Count() == 1)
            {
                var _user = result.First();
                var token = _sessionService.CreatToken(_user.username);
                _sessionService.Sessions.Add(token, _user.id);
                return Ok(new { token, _user.username });
            }
            return NotFound(user);
        }

        // PUT api/<TokenController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TokenController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
