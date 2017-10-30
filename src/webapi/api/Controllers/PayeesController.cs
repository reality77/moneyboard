using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using dal;

namespace api.Controllers
{
    [Produces("application/json")]
    [Route("payees")]
    public class PayeesController : MoneyboardController
    {
        protected readonly dal_postgres.MoneyboardPostgresContext _db;

        public PayeesController(dal_postgres.MoneyboardPostgresContext db)
        {
            _db = db;
        }

        // GET: Payees
        [HttpGet]
        public IEnumerable<dto.Payee> Get()
        {
            return _db.Payees.ConvertToDtoList<dto.Payee>(_db);
        }

        // GET: Payees/5
        [HttpGet("{id}", Name = "PayeesGet")]
        public dto.Payee Get(int id)
        {
            return _db.GetPayee(id)
                .CreateDto<dto.Payee>(_db);
        }

        // POST: Payees
        [HttpPost]
        public IActionResult Post([FromBody] string payeeName)
        {
            if(string.IsNullOrWhiteSpace(payeeName))
                return BadRequest("Payee name is required");

            var dbPayee = new dal.models.Payee();
            dbPayee.Name = payeeName;

            _db.Payees.Add(dbPayee);
            _db.SaveChanges();

            return Ok(dbPayee.CreateDto<dto.Payee>(_db));
        }

        // PUT: Payees/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] string payeeName)
        {
            //TODO : test payee id
            /*payee.ID = id;

            var dbPayee = _db.GetPayee(id);
            //TODO : test null

            dbPayee.UpdateFrom(payee, _db);
            _db.SaveChanges();*/
            return NoContent();
        }

        // DELETE: Payees/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var dbPayee = _db.GetPayee(id);
            //TODO : test null

            _db.Payees.Remove(dbPayee);
            _db.SaveChanges();
        }
    }
}