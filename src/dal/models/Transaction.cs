using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace dal.models
{
    public partial class Transaction
    {
        [Key]
        public int ID { get; set; }

        public int AccountID {get; set; }
        public Account Account { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public string Caption { get; set; }

        [Required]
        public dto.ETransactionType Type { get; set; }

        [Required]
        public DateTime UserDate { get; set; }

        public int? CategoryID {get; set; }
        public Category Category { get; set; }

        public int? PayeeID {get; set; }
        public Payee Payee { get; set; }
    }
}
