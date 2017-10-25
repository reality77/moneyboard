using System;
using System.Collections.Generic;
using System.Text;

namespace dto.transactions_view
{
    public class TransactionRow
    {
        public int RowId { get; set; }

        public Transaction Transaction { get; set; }
    }

    public class AccountTransactionRow : TransactionRow
    {
        public CurrencyNumber Balance { get; set; }
    }
}