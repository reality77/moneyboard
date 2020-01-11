using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using dal;

namespace api.Controllers
{
    [ApiController]
    [Route("payees")]
    [Produces("application/json")]
    public class PayeesController : MoneyboardController
    {
        protected readonly dal.Model.MoneyboardContext _db;

        public PayeesController(dal.Model.MoneyboardContext db)
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

            var dbPayee = _db.Payees.SingleOrDefault(p => p.Name.ToLower().Trim() == payeeName.Trim().ToLower());

            if(dbPayee != null)
                return BadRequest("Payee name already exists");

            dbPayee = new dal.Model.Payee();
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

        // GET: Payees/statistics
        [HttpGet("monthlystatisticsbypayee")]
        public dto.statistics.CurrencyNumberStatisticsByDate MonthlyStatisticsByPayee(int accountId, int payeeId, DateTime? dateStart = null, DateTime? dateEnd = null)
        {
            if (dateEnd == null)
                dateEnd = DateTime.Today;
            else
                dateEnd = dateEnd.Value.Date;

            if (dateStart == null)
                dateStart = dateEnd.Value.AddYears(-1);
            else
                dateStart = dateStart.Value.Date;

            var account = _db.GetAccount(accountId);
            var payee = _db.GetPayee(payeeId);

            var data = _db.Transactions.Where(t => t.PayeeId == payeeId)
                .GroupBy(t => new { t.Date.Year, t.Date.Month})
                .Select(g => new { Key = g.Key, Value = g.Sum(t => t.Amount) });

            var stat = new dto.statistics.CurrencyNumberStatisticsByDate();

            stat.Init();

            int serieIndex = stat.AddSerie("Montant");

            foreach (var item in data)
            {
                int idx = stat.SetXValue(new DateTime(item.Key.Year, item.Key.Month, 1));
                
                stat.SetValue(idx, serieIndex, new dto.CurrencyNumber
                    {
                        Currency = account.Currency,
                        Value = item.Value,
                    });
            }

            stat.GenerateDataPoints();

            return stat;
        }

    }
}