using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace dal.models
{
    public partial class ImportPayeeSelection
    {
        [Key]
        public int ID { get; set; }

        public int ImportRegexId { get; set; }
        public ImportRegex ImportRegex { get; set; }

        [Required]
        public string ImportedCaption { get; set; }

        public int PayeeId { get; set; }
        public Payee Payee { get; set; }

        public int? CategoryId { get; set; }
        public Category Category { get; set; }

        public string TransactionCaption { get; set; }
    }
}
