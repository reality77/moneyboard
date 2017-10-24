using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace api.Importer
{
    [DataContract]
    public class ImportAccount
    {
        [IgnoreDataMember]
        public string OriginalAccountName { get; set; }
        
        [DataMember]
        public string AccountName { get; set; }

        [DataMember]
        public string AccountNumber { get; set; }

        [DataMember]
        public string Currency { get; set; }

        [DataMember]
        public decimal? LedgerBalanceAmount { get; set; }

        [DataMember]
        public DateTime? LedgerBalanceDate { get; set; }

        [DataMember]
        public decimal? AvailableBalanceAmount { get; set; }

        [DataMember]
        public DateTime? AvailableBalanceDate { get; set; }

        [IgnoreDataMember]
        public decimal ImportInitialBalance { get; set; }

        [IgnoreDataMember]
        public List<ImportTransaction> Transactions { get; set; }

        public ImportAccount()
        {
            this.Transactions = new List<ImportTransaction>();
            this.Currency = "EUR";
        }
    }
}
