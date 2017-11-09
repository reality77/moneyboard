using System;
using System.Collections.Generic;
using System.Linq;

namespace dto.import
{
    public class ImportedAccount
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public ECurrency Currency { get; set; }

        public List<ImportedTransaction> Transactions { get; set; }

        public ImportedAccount()
        {
            this.Transactions = new List<ImportedTransaction>();
        }
    }
}