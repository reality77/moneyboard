using System;
using System.Collections.Generic;
using System.Text;

namespace dto.transactions_view
{
    public class TransactionsView
    {
        public IEnumerable<TransactionRow> Transactions { get; set; }
    }
}