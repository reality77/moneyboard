using System;
using System.Collections.Generic;
using System.Text;

namespace dto.transactions_view
{
    public class TransactionsView
    {
        public int PageId { get; set; }
        public int PageCount { get; set; }

        public IEnumerable<TransactionRow> Transactions { get; set; }
    }
}