using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using PercobaanApi1.Models;
using System.Collections.Generic;

namespace PercobaanApi1.Controllers
{
    [Route("api/person")]
    [ApiController]
    public class PersonController : Controller
    {
        private readonly string __constr;

        public PersonController(IConfiguration configuration)
        {
            __constr = configuration.GetConnectionString("WebApiDatabase");
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public ActionResult<Person> ListPerson()
        {
            PersonContext context = new PersonContext(this.__constr);
            List<Person> list = context.ListPerson();
            return Ok(list);
        }

        [Authorize(Roles = "User")]
        [HttpGet("{id}")]
        public ActionResult<Person> GetMuridById(int id)
        {
            PersonContext context = new PersonContext(this.__constr);
            var person = context.GetMuridById(id);
            if (person == null)
            {
                return NotFound("Data murid tidak ditemukan");
            }
            return Ok(person);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult AddMurid(Person person)
        {
            PersonContext context = new PersonContext(this.__constr);
            if (context.AddMurid(person))
            {
                return Ok("Data murid berhasil ditambahkan");
            }
            return BadRequest("Data murid gagal ditambahkan");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("Update/{id}")]
        public ActionResult UpdateMurid(int id, Person person)
        {
            PersonContext context = new PersonContext(this.__constr);
            person.id_person = id;
            if (context.UpdateMurid(person))
            {
                return Ok("Data murid berhasil diubah");
            }
            return BadRequest("Data murid gagal diubah");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{id}")]
        public ActionResult DeleteMurid(int id)
        {
            PersonContext context = new PersonContext(this.__constr);
            if (context.DeleteMurid(id))
            {
                return Ok("Data murid berhasil dihapus");
            }
            return BadRequest("Data murid gagal dihapus");


        }
    }
}
