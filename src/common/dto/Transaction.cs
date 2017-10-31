using System;
using System.Collections.Generic;
using System.Text;

namespace dto
{
    public class Transaction : IDtoObject
    {
        public int ID { get; set; }
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string Caption { get; set; }
        public CurrencyNumber Amount { get; set; }
        public ETransactionType Type { get; set; }
        public DateTime Date { get; set; }
        public DateTime? UserDate { get; set; }

        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? PayeeId { get; set; }
        public string PayeeName { get; set; }

        public string Comment { get; set; }
        public string ImportedTransactionCaption { get; set; }
        public string ImportedTransactionHash { get; set; }
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
