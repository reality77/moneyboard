using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using dal;
using dal.models;
using dto.transactions_view;

namespace api.Controllers
{
    [Produces("application/json")]
    [Route("TransactionsView")]
    public class TransactionsViewController : MoneyboardController
    {
        protected readonly MoneyboardContext _db;

        public TransactionsViewController(MoneyboardContext db)
        {
            _db = db;
        }

        // GET: api/TransactionsView/Account/{accountId}
        [HttpGet("Account")]
        public TransactionsView GetFromAccountId(int accountId)
        {
            //TODO
            return null;
        }
    }
}
