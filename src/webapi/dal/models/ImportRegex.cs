using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.RegularExpressions;

namespace dal.models
{
    public partial class ImportRegex
    {
        Regex _regex;

        [Key]
        public int ID { get; set; }

        [Required]
        public string RegexString { get; set; }

        [Required]
        public ICollection<ImportPayeeSelection> ImportPayeeSelections { get; set; }
        
        public dto.ETransactionType TransactionType { get; set; }

        [NotMapped]
        public Regex Regex 
        { 
            get 
            { 
                if(_regex == null && !string.IsNullOrEmpty(this.RegexString))
                    _regex = new Regex(this.RegexString);

                return _regex;
            }
        }
    }
}
