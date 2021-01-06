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
    public class ExamTranscriptsController : DefaultController
    {
        private readonly ExamTranscriptsRepository _ExamTranscriptsRepository;

        public ExamTranscriptsController(ExamTranscriptsRepository ExamTranscriptsRepository, IHttpContextAccessor context) : base(context)
        {
            _ExamTranscriptsRepository = ExamTranscriptsRepository;
        }

        // GET: api/<CoursesController>
        [HttpGet]
        public async Task<IEnumerable<ExamTranscripts>> GetAsync()
        {
            return await _ExamTranscriptsRepository.GetList(_uid);
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
