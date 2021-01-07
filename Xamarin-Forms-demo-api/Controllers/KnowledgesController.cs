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
    public class KnowledgesController : DefaultController
    {
        private readonly KnowledgesRepository _KnowledgesRepository;

        public KnowledgesController(KnowledgesRepository KnowledgesRepository, IHttpContextAccessor context) : base(context)
        {
            _KnowledgesRepository = KnowledgesRepository;
        }

        // GET: api/<CoursesController>
        [HttpGet]
        public async Task<IEnumerable<Knowledges>> GetAsync([FromQuery] int p)
        {
            return await _KnowledgesRepository.GetList(maxId: p, limit: 5);
        }

        // GET api/<CoursesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CoursesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CoursesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CoursesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
