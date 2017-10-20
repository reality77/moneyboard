using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using dal.models;
using dal;

namespace api.Controllers
{
    [Produces("application/json")]
    [Route("Accounts")]
    public class AccountsController : MoneyboardController
    {
        protected readonly MoneyboardContext _db;

        public AccountsController(MoneyboardContext db)
        {
            _db = db;
        }

        // GET: api/Accounts
        [HttpGet]
        public IEnumerable<Account> Get()
        {
            return _db.Accounts;
        }

        // GET: api/Accounts/5
        [HttpGet("{id}", Name = "Get")]
        public Account Get(int id)
        {
            return _db.Accounts.SingleOrDefault(a => a.ID == id);
        }

        // POST: api/Accounts
        [HttpGet("debugadd")]
        public Account DebugAdd(string name)
        {
            Account acc = new Account
            {
                AccountName = name,
                Currency = ECurrency.EUR,
                InitialBalance = 0,
                Balance = 0,
            };

            _db.Accounts.Add(acc);
            _db.SaveChanges();

            return acc;
        }

        // POST: api/Accounts
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }
        
        // PUT: api/Accounts/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
