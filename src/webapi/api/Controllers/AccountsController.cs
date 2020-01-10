using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using dal;
using dal.models;

namespace api.Controllers
{
    [ApiController]
    [Route("accounts")]
    [Produces("application/json")]
    public class AccountsController : MoneyboardController
    {
        protected readonly dal_postgres.MoneyboardPostgresContext _db;

        public AccountsController(dal_postgres.MoneyboardPostgresContext db)
        {
            _db = db;
        }

        // GET: api/Accounts
        [HttpGet]
        public IEnumerable<dto.Account> Get()
        {
            return _db.Accounts.ConvertToDtoList<dto.Account>(_db);
        }

        // GET: api/Accounts/5
        [HttpGet("{id}", Name = "AccountsGet")]
        public dto.Account Get(int id)
        {
            return _db.GetAccount(id)
                .CreateDto<dto.Account>(_db);
        }

        // POST: api/Accounts
        [HttpGet("debugadd")]
        public dto.Account DebugAdd(string name)
        {
            var dbaccount = new Account
            {
                Name = name,
                Currency = dto.ECurrency.EUR,
                InitialBalance = 0,
                Balance = 0,
            };

            _db.Accounts.Add(dbaccount);
            _db.SaveChanges();

            return dbaccount.CreateDto<dto.Account>(_db);
        }

        // POST: api/Accounts
        [HttpPost]
        public void Post([FromBody] dto.Account account)
        {
            var dbaccount = new Account();
            dbaccount.UpdateFrom(account, _db);

            _db.Accounts.Add(dbaccount);
            _db.SaveChanges();
        }
        
        // PUT: api/Accounts/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] dto.Account account)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        // GET: accounts/monthlystatistics
        [HttpGet("{accountId}/monthlystatistics")]
        public dto.statistics.CurrencyNumberStatisticsByDate StatisticsByCategory(int accountId, DateTime? dateStart = null, DateTime? dateEnd = null)
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

            var queryBase = _db.Transactions.Where(t => t.AccountId == accountId)
                .GroupBy(t => new { t.Date.Year, t.Date.Month})
                .Select(g => new { Key = g.Key, Value = g.Sum(t => t.Amount) });

            var stat = new dto.statistics.CurrencyNumberStatisticsByDate();

            stat.Init();

            int serieIndex = stat.AddSerie("Revenus");
            var dataRevenues = queryBase.Where(t => t.Value > 0);

            foreach (var item in dataRevenues)
            {
                int idx = stat.SetXValue(new DateTime(item.Key.Year, item.Key.Month, 1));
                
                stat.SetValue(idx, serieIndex, new dto.CurrencyNumber
                    {
                        Currency = account.Currency,
                        Value = item.Value,
                    });
            }

            serieIndex = stat.AddSerie("Dépenses");
            var dataExpenses = queryBase.Where(t => t.Value < 0);

            foreach (var item in dataExpenses)
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
