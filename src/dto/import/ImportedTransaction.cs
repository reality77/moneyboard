using System;
using System.Collections.Generic;
using System.Linq;

namespace dto.import
{
    public class ImportedTransaction
    {
        public DateTime TransactionDate { get; set; }

        public string CaptionOrPayee { get; set; }

        public string Category { get; set; }

        public string TransferTarget { get; set; }

        public decimal Amount { get; set; }

        public string Memo { get; set; }

        public string Number { get; set; }

        public string Error { get; set; }
    }
}
