using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using dto;

namespace dal.Model
{
    public partial class ImportRegex
    {
        Regex _regex;

        public ImportRegex()
        {
            ImportPayeeSelections = new HashSet<ImportPayeeSelection>();
        }

        public int Id { get; set; }
        public string RegexString { get; set; }
        public ETransactionType TransactionType { get; set; }

        public virtual ICollection<ImportPayeeSelection> ImportPayeeSelections { get; set; }

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
