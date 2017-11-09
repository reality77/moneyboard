using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using dal;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Produces("application/json")]
    [Route("statistics")]
    public class StatisticsController : Controller
    {
        protected readonly dal_postgres.MoneyboardPostgresContext _db;

        public StatisticsController(dal_postgres.MoneyboardPostgresContext db)
        {
            _db = db;
        }

        // GET: statistics/monthly
        [HttpGet("monthly")]
        public dto.statistics.DateStatistics MonthlyStatistics(int accountId, int? idOrigin, dto.statistics.StatisticsOrigin origin, DateTime? dateStart = null, DateTime? dateEnd = null)
        {
            if (dateEnd == null)
                dateEnd = DateTime.Today;
            else
                dateEnd = dateEnd.Value.Date;

            if (dateStart == null)
                dateStart = dateEnd.Value.AddYears(-1);
            else
                dateStart = dateStart.Value.Date;

            var query = _db.Transactions.AsQueryable();

            if (idOrigin != null)
            {
                switch (origin)
                {
                    case dto.statistics.StatisticsOrigin.Category:
                        query = query.Where(t => t.CategoryId == idOrigin);
                        break;
                    case dto.statistics.StatisticsOrigin.Payee:
                        query = query.Where(t => t.PayeeId == idOrigin);
                        break;
                }
            }

            var account = _db.GetAccount(accountId);

            var data = query.GroupBy(t => new { Year = t.Date.Year, Month = t.Date.Month })
                .Select(g => new { Key = g.Key, Value = g.Sum(t => t.Amount) });

            var stat = new dto.statistics.DateStatistics();

            foreach (var item in data)
            {
                stat.Data.Add(new DateTime(item.Key.Year, item.Key.Month, 1),
                    new dto.CurrencyNumber
                    {
                        Currency = account.Currency,
                        Value = item.Value,
                    });
            }

            return stat;
        }
    }
}
