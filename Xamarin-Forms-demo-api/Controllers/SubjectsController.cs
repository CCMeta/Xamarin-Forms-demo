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
    public class SubjectsController : DefaultController
    {
        private readonly SubjectsRepository _SubjectsRepository;

        public SubjectsController(SubjectsRepository SubjectsRepository, IHttpContextAccessor context) : base(context)
        {
            _SubjectsRepository = SubjectsRepository;
        }

        // GET: api/<SubjectsController>
        [HttpGet]
        public async Task<IEnumerable<Subjects>> GetAsync([FromQuery] int p)
        {
            return await _SubjectsRepository.GetList(maxId: p, limit: 5);
        }

        // GET api/<SubjectsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<SubjectsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<SubjectsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SubjectsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
