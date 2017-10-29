using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace dal.models
{
    public partial class Account
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal InitialBalance { get; set; }

        [Required]
        public decimal Balance { get; set; }

        [Required]
        public dto.ECurrency Currency { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
    }
}
