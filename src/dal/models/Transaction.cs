using System;
using System.Collections.Generic;
using System.Text;

namespace dal.models
{
    public class Transaction
    {
        public int ID { get; set; }
        public Account Account { get; set; }
        public decimal Amount { get; set; }
        public ETransactionType Type { get; set; }
        public DateTime UserDate { get; set; }

        public Category Category { get; set; }
        public Payee Payee { get; set; }
    }

    public enum ETransactionType : int
    {
        Normal = 0,
        Transfer = 1,
    }
}
