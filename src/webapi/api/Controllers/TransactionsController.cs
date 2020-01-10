using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using dal;
using dal.models;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [ApiController]
    [Route("Transactions")]
    [Produces("application/json")]
    public class TransactionsController : MoneyboardController
    {
        protected readonly dal_postgres.MoneyboardPostgresContext _db;

        public TransactionsController(dal_postgres.MoneyboardPostgresContext db)
        {
            _db = db;
        }

        // GET: api/Transactions?accountId
        [HttpGet]
        public IEnumerable<dto.Transaction> GetFromAccountId(int accountId)
        {
            return _db.Transactions
                .Include(trx => trx.Account)
                .Include(trx => trx.Category)
                .Include(trx => trx.Payee)
                .Where(t => t.Account.ID == accountId)
                .ConvertToDtoList<dto.Transaction>(_db);
        }

        // GET: api/Transactions/5
        [HttpGet("{id}", Name = "TransactionsGet")]
        public dto.Transaction Get(int id)
        {
            return _db.GetTransaction(id)
                .CreateDto<dto.Transaction>(_db);
        }

        // POST: api/Transactions
        [HttpPost]
        public void Post([FromBody] dto.Transaction transaction)
        {
            var dbtransaction = new Transaction();
            dbtransaction.UpdateFrom(transaction, _db);

            _db.Transactions.Add(dbtransaction);
            _db.SaveChanges();
        }
        
        // PUT: api/Transactions/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] dto.Transaction account)
        {
        }
        
        // DELETE: api/Transactions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            
        }
    }
}
