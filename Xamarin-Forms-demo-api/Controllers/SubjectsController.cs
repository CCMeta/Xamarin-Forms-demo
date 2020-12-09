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
    public class SubjectsController : ControllerBase
    {
        private readonly SubjectsRepository _ProductRepository;

        public SubjectsController(SubjectsRepository productRepository)
        {
            _ProductRepository = productRepository;
        }

        // GET: api/<SubjectsController>
        [HttpGet]
        public async Task<IEnumerable<Subjects>> GetAsync()
        {
            string page = HttpContext.Request.Query.TryGetValue("fuck", out var StringValues) ? StringValues.ToString() : "1";
            return await _ProductRepository.GetSubjects(page: Convert.ToInt32(page), limit: 5);
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
