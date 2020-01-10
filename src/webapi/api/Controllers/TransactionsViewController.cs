using Microsoft.AspNetCore.Mvc;
using dto.transactions_view;

namespace api.Controllers
{
    [ApiController]
    [Route("transactions_view")]
    [Produces("application/json")]
    public class TransactionsViewController : MoneyboardController
    {
        protected readonly dal.Model.MoneyboardContext _db;

        public TransactionsViewController(dal.Model.MoneyboardContext db)
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
