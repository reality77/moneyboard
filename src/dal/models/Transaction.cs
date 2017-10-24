﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace dal.models
{
    public partial class Transaction
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public Account Account { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public dto.ETransactionType Type { get; set; }

        [Required]
        public DateTime UserDate { get; set; }

        public Category Category { get; set; }

        public Payee Payee { get; set; }
    }
}
