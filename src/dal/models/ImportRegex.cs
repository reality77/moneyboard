using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace dal.models
{
    public partial class ImportRegex
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Regex { get; set; }

        [Required]
        public ICollection<ImportPayeeSelection> ImportPayeeSelections { get; set; }
        
        public dto.ETransactionType TransactionType { get; set; }
    }
}
