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
        private readonly SubjectsRepository _SubjectsController;

        public SubjectsController(SubjectsRepository SubjectsController, IHttpContextAccessor context) : base(context)
        {
            _SubjectsController = SubjectsController;
        }

        // GET: api/<SubjectsController>
        [HttpGet]
        public async Task<IEnumerable<Subjects>> GetAsync()
        {
            string page = HttpContext.Request.Query.TryGetValue("p", out var StringValues) ? StringValues.ToString() : "1";
            return await _SubjectsController.GetList(page: Convert.ToInt32(page), limit: 5);
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
