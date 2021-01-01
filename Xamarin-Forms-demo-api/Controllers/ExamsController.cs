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
    public class ExamsController : DefaultController
    {
        private readonly ExamsRepository _ExamsRepository;

        public ExamsController(ExamsRepository ExamsRepository, IHttpContextAccessor context) : base(context)
        {
            _ExamsRepository = ExamsRepository;
        }

        // GET: api/<CoursesController>
        [HttpGet]
        public async Task<IEnumerable<Exams>> GetAsync()
        {
            string page = HttpContext.Request.Query.TryGetValue("p", out var StringValues) ? StringValues.ToString() : "1";
            return await _ExamsRepository.GetList(page: Convert.ToInt32(page), limit: 5);
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
