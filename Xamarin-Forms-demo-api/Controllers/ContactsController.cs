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
    public class ContactsController : DefaultController
    {
        private readonly ContactsRepository _contactsRepository;

        public ContactsController(ContactsRepository contactsRepository, IHttpContextAccessor context) : base(context)
        {
            _contactsRepository = contactsRepository;
        }

        // GET: api/<ContactsController>
        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            var result = await _contactsRepository.GetList(_uid);
            return Ok(result);
        }

        // GET api/<ContactsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ContactsController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Contacts contacts)
        {
            var partner_id = contacts.partner_id == 0 ? throw new ApplicationException("BAD QUERY") : contacts.partner_id;
            var result = await _contactsRepository.Post(partner_id, _uid);
            return Ok(result);
        }

        // PUT api/<ContactsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ContactsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
