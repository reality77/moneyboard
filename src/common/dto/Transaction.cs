using System;
using System.Collections.Generic;
using System.Text;

namespace dto
{
    public class Transaction : IDtoObject
    {
        public int ID { get; set; }
        public Account Account { get; set; }
        public string Caption { get; set; }
        public CurrencyNumber Amount { get; set; }
        public ETransactionType Type { get; set; }
        public DateTime UserDate { get; set; }

        public Category Category { get; set; }
        public Payee Payee { get; set; }
    }

    public enum ETransactionType : int
    {
        Unknown = 0,
        
        // Paiment
        Payment = 1,

        //Virement bancaire
        Transfer = 2,

        // Retrait d'espèces
        Withdrawal = 3,

        // Prélèvement bancaire
        Debit = 4
    }
}
