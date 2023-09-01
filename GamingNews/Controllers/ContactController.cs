using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ContactsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly List<Contact> _contacts = new List<Contact>
        {
            new Contact { Id = 1, Name = "John Doe", Email = "john@example.com" },
            new Contact { Id = 2, Name = "Jane Smith", Email = "jane@example.com" }
        };

        // GET: api/contacts
        [HttpGet]
        public ActionResult<IEnumerable<Contact>> Get()
        {
            return Ok(_contacts);
        }

        // GET: api/contacts/1
        [HttpGet("{id}")]
        public ActionResult<Contact> Get(int id)
        {
            var contact = _contacts.FirstOrDefault(c => c.Id == id);
            if (contact == null)
            {
                return NotFound();
            }
            return Ok(contact);
        }

        // POST: api/contacts
        [HttpPost]
        public ActionResult<Contact> Post([FromBody] Contact contact)
        {
            if (contact == null)
            {
                return BadRequest();
            }

            contact.Id = _contacts.Max(c => c.Id) + 1;
            _contacts.Add(contact);
            return CreatedAtAction(nameof(Get), new { id = contact.Id }, contact);
        }

        // PUT: api/contacts/1
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Contact contact)
        {
            var existingContact = _contacts.FirstOrDefault(c => c.Id == id);
            if (existingContact == null)
            {
                return NotFound();
            }

            existingContact.Name = contact.Name;
            existingContact.Email = contact.Email;
            return NoContent();
        }

        // DELETE: api/contacts/1
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var contact = _contacts.FirstOrDefault(c => c.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            _contacts.Remove(contact);
            return NoContent();
        }
    }

    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
