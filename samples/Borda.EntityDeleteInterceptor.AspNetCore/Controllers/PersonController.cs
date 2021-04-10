using System.Linq;
using Borda.EntityDeleteInterceptor.AspNetCore.Contexts;
using Borda.EntityDeleteInterceptor.AspNetCore.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Borda.EntityDeleteInterceptor.AspNetCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public PersonController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Persons.ToList());
        }

        [HttpPost]
        public IActionResult Create(Person person)
        {
            _context.Persons.Add(person);
            _context.SaveChanges();
            return Ok(person);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
           var person = _context.Persons.Find(id);
           _context.Persons.Remove(person);
           _context.SaveChanges();

            return NoContent();
        }
    }
}