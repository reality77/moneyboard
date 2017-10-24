using System;
using System.Collections.Generic;
using System.Text;

namespace dto
{
    public class Transaction : IDtoObject
    {
        public int ID { get; set; }
        public Account Account { get; set; }
        public CurrencyNumber Amount { get; set; }
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
