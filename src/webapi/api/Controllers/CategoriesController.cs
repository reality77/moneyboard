using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dal;

namespace api.Controllers
{
    [Produces("application/json")]
    [Route("categories")]
    public class CategoriesController : MoneyboardController
    {
        protected readonly dal_postgres.MoneyboardPostgresContext _db;

        public CategoriesController(dal_postgres.MoneyboardPostgresContext db)
        {
            _db = db;
        }

        // GET: Categorys
        [HttpGet]
        public IEnumerable<dto.Category> Get()
        {
            return _db.Categories.ConvertToDtoList<dto.Category>(_db);
        }

        // GET: Categorys/5
        [HttpGet("{id}", Name = "CategorysGet")]
        public dto.Category Get(int id)
        {
            return _db.GetCategory(id)
                .CreateDto<dto.Category>(_db);
        }

        // POST: Categorys
        [HttpPost]
        public ActionResult Post([FromBody] string categoryName)
        {
            if(string.IsNullOrWhiteSpace(categoryName))
                return BadRequest("Category name is required");

            var dbCategory = _db.Categories.SingleOrDefault(p => p.Name.ToLower().Trim() == categoryName.Trim().ToLower());

            if (dbCategory != null)
                return BadRequest("Category name already exists");

            dbCategory = new dal.models.Category();
            dbCategory.Name = categoryName;

            _db.Categories.Add(dbCategory);
            _db.SaveChanges();
        
            return Ok(dbCategory.CreateDto<dto.Category>(_db));
        }

        // PUT: Categorys/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string categoryName)
        {
            //TODO : test category id
            /*category.ID = id;

            var dbCategory = _db.GetCategory(id);
            //TODO : test null

            dbCategory.UpdateFrom(category, _db);
            _db.SaveChanges();*/
        }

        // DELETE: Categorys/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var dbCategory = _db.GetCategory(id);
            //TODO : test null

            _db.Categories.Remove(dbCategory);
            _db.SaveChanges();
        }

        // GET: Categorys/statistics
        [HttpGet("monthlystatistics")]
        [HttpGet("{categoryId}/monthlystatistics")]
        public dto.statistics.CurrencyNumberStatisticsByDate MonthlyStatisticsByCategory(int? categoryId, int accountId, DateTime? dateStart = null, DateTime? dateEnd = null)
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

            /* EF Core 2 does not support Group By (waiting for 2.1 ?)
            var query = _db.Transactions.AsQueryable();

            query = query.Include(t => t.Category);

            if(categoryId != null)
                query = query.Where(t => t.CategoryId == categoryId);

            var x = query
                .GroupBy(t => new 
                    { 
                        t.CategoryId, 
                        t.Category.Name, 
                        t.Date.Year,
                        t.Date.Month
                    }).ToList();

            var data = query
                .GroupBy(t => new 
                    { 
                        t.CategoryId, 
                        t.Category.Name, 
                        t.Date.Year,
                        t.Date.Month
                    })
                .Select(g => new { g.Key, Value = g.Sum(t => t.Amount) })
                .ToList();
            */

            var where = "";

            if(categoryId != null) {
                where = Environment.NewLine + $@"WHERE c.""ID"" = {categoryId.Value}" + Environment.NewLine;
            }

            /* Temporary : SQL Direct [EF Core 2 does not support Group By (waiting for 2.1 ?)]*/
            /* NOTE : AsNoTracking is REQUIRED else the same records are returned (because of same ID) !!!! */
            string sql = $@"SELECT c.""ID"" as ""CategoryId"", c.""Name"" as ""CategoryName"", CAST(date_part('year', t.""Date"") AS INTEGER) as ""Year"",  CAST(date_part('month', t.""Date"") AS INTEGER) as ""Month"", SUM(t.""Amount"") as ""Value"", CAST(0 AS INTEGER) as ""ID""
FROM ""Transactions"" t
INNER JOIN ""Categories"" c ON t.""CategoryId"" = c.""ID"" {where}
GROUP BY c.""ID"", c.""Name"", date_part('year', t.""Date""), date_part('month', t.""Date"")
ORDER BY c.""ID"", date_part('year', t.""Date""), date_part('month', t.""Date"")
";
            var data = _db.MonthlyCategoryStats.FromSql(sql)
                .AsNoTracking()
                .ToList();

            Dictionary<int, int> dicSerieIndexByCategoryId = new Dictionary<int, int>();

            var stat = new dto.statistics.CurrencyNumberStatisticsByDate();
            stat.Init();

            int nullserieIndex = -1;

            if(categoryId == null)
                nullserieIndex = stat.AddSerie("(aucune)"); // Serie for null categories

            foreach (var item in data)
            {
                int serieIndex;

                if(item.CategoryId == null)
                    serieIndex = nullserieIndex;
                else if(!dicSerieIndexByCategoryId.ContainsKey(item.CategoryId.Value))
                {
                    serieIndex = stat.AddSerie(item.CategoryName);
                    dicSerieIndexByCategoryId[item.CategoryId.Value] = serieIndex;
                }
                else
                    serieIndex = dicSerieIndexByCategoryId[item.CategoryId.Value];

                int idx = stat.SetXValue(new DateTime(item.Year, item.Month, 1));
                
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