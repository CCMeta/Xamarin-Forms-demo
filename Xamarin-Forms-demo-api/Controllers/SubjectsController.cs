using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin_Forms_demo_api.Models;
using MySqlConnector;

namespace Xamarin_Forms_demo_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly MySqlConnection _dbcontext;

        public SubjectsController(MySqlConnection dbContext)
        {
            _dbcontext = dbContext;
            _dbcontext.Open();
        }

        // GET: api/<SubjectsController>
        [HttpGet]
        public async Task<IEnumerable<string>> GetAsync()
        {
            //var fuck = _dbcontext.Subjects.FromSqlRaw("SELECT * FROM subjects LIMIT 5").ToList();
            //var fuck2 = JsonSerializer.Serialize(fuck);
            using (var cmd = new MySqlCommand())
            {
                cmd.Connection = _dbcontext;
                cmd.CommandText = "SELECT * FROM subjects LIMIT @p";
                cmd.Parameters.AddWithValue("p", 6);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    var subjects = new List<string>();
                    while (await reader.ReadAsync())
                    {
                        subjects.Add(reader.GetString("vname"));
                    }
                    return subjects;
                }
            }
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
