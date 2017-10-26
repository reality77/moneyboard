using System;
using System.Collections.Generic;
using System.Linq;

namespace dto.import
{
    public class ImportedAccount
    {
        public List<ImportedTransaction> Transactions { get; set; }
    }
}