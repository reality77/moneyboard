using System;
using System.Collections.Generic;
using dto;

namespace dal.Model
{
    public partial class Transaction
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public string Caption { get; set; }
        public int? CategoryId { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }
        public string ImportedTransactionCaption { get; set; }
        public string ImportedTransactionHash { get; set; }
        public int? PayeeId { get; set; }
        public ETransactionType Type { get; set; }
        public DateTime? UserDate { get; set; }

        public virtual Account Account { get; set; }
        public virtual Category Category { get; set; }
        public virtual Payee Payee { get; set; }
    }
}
