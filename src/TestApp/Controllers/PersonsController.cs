using EmbeddedServer;
using EmbeddedServer.Attributes;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using TestApp.Models;
using System.IO;

namespace TestApp.Controllers
{
    public class PersonsController : Controller
    {
        private List<Person> _persons = new List<Person>()
        {
            new Person {Id = 1, Name = "Johnny Appleseed"},
            new Person {Id = 2, Name = "Euclid Huntersworth"},
            new Person {Id = 3, Name = "Foobar Forima"},
            new Person {Id = 4, Name = "Saladin Ayoubi"}
        };


        [HttpGet("persons")]
        public IActionResult GetPersons()
        {
            return Ok(_persons);
        }

        [HttpGet("persons/{id:int}")]
        public IActionResult GetPerson(int id)
        {
            var person = _persons.FirstOrDefault(x => x.Id == id);
            if (person == null)
                return NotFound($"No person with ID: {id} found.");
            return Ok(person);
        }

        [HttpGet("persons/by-name/{name}")]
        public IActionResult GetPerson(string name)
        {
            var person = _persons.FirstOrDefault(x => x.Name == name);
            if (person == null)
                return NotFound($"No person with name: {name} found.");
            return Ok(person);
        }

        [HttpGet("persons/by-name")]
        public IActionResult GetPerson2(string name)
        {
            var person = _persons.FirstOrDefault(x => x.Name == name);
            if (person == null)
                return NotFound($"No person with name: {name} found.");
            return Ok(person);
        }

        [HttpGet("persons/file")]
        public IActionResult GetFile()
        {
            return File(@"C:\tmp\NTB.docx");
        }
    }
}
