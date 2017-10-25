using Microsoft.AspNetCore.Mvc;
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
            var result = business.AccountTransactionsViewsCreator.GenerateAccountView(accountId, pageId, itemsPerPage, _db);
            return result; 
        }
    }
}
