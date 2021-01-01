﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin_Forms_demo_api.Models;

namespace Xamarin_Forms_demo_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : DefaultController
    {
        private readonly CoursesRepository _CoursesRepository;

        public CoursesController(CoursesRepository CoursesRepository, IHttpContextAccessor context) : base(context)
        {
            _CoursesRepository = CoursesRepository;
        }

        // GET: api/<CoursesController>
        [HttpGet]
        public async Task<IEnumerable<Courses>> GetAsync()
        {
            string page = HttpContext.Request.Query.TryGetValue("p", out var StringValues) ? StringValues.ToString() : "1";
            var fuck = await _CoursesRepository.GetList(page: Convert.ToInt32(page), limit: 5);
            return fuck;
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