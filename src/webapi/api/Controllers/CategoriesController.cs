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
        [HttpGet("statisticsbycategory")]
        public dto.statistics.DateStatistics StatisticsByCategory(int accountId, int categoryId, DateTime? dateStart = null, DateTime? dateEnd = null)
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
            var category = _db.GetCategory(categoryId);

            var data = _db.Transactions.Where(t => t.CategoryId == categoryId)
                .GroupBy(t => t.Date.Year * 100 + t.Date.Month)
                .Select(g => new Tuple<int, decimal>(g.Key, g.Sum(t => t.Amount)));

            var stat = new dto.statistics.DateStatistics();

            foreach (var item in data)
            {
                stat.Data.Add(item.Item1,
                    new dto.CurrencyNumber
                    {
                        Currency = account.Currency,
                        Value = item.Item2,
                    });
            }

            return stat;
        }
    }
}