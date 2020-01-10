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
    [ApiController]
    [Route("categories")]
    [Produces("application/json")]
    public class CategoriesController : MoneyboardController
    {
        protected readonly dal.Model.MoneyboardContext _db;

        public CategoriesController(dal.Model.MoneyboardContext db)
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

            dbCategory = new dal.Model.Category();
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

            var query = _db.Transactions.AsQueryable();

            query = query.Include(t => t.Category);

            if(categoryId != null)
                query = query.Where(t => t.CategoryId == categoryId);

            var data = query
                .GroupBy(t => new 
                    { 
                        t.Category, 
                        t.Date.Year,
                        t.Date.Month
                    })
                .Select(g => new { g.Key, Value = g.Sum(t => t.Amount) })
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

                if(item.Key.Category == null)
                    serieIndex = nullserieIndex;
                else if(!dicSerieIndexByCategoryId.ContainsKey(item.Key.Category.Id))
                {
                    serieIndex = stat.AddSerie(item.Key.Category.Name);
                    dicSerieIndexByCategoryId[item.Key.Category.Id] = serieIndex;
                }
                else
                    serieIndex = dicSerieIndexByCategoryId[item.Key.Category.Id];

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