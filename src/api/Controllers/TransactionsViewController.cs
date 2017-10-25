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
    [Route("transactions_view")]
    public class TransactionsViewController : MoneyboardController
    {
        protected readonly dal_postgres.MoneyboardPostgresContext _db;

        public TransactionsViewController(dal_postgres.MoneyboardPostgresContext db)
        {
            _db = db;
        }

        // GET: transactions_view/account/{accountId}
        [HttpGet("account/{accountId}")]
        public TransactionsView GetFromAccountId(int accountId, int pageId = 0, int itemsPerPage = 10)
        {
            return business.AccountTransactionsViewsCreator.GenerateAccountView(accountId, pageId, itemsPerPage, _db);
        }
    }
}
