using System;
using System.Collections.Generic;

namespace dal.Model
{
    public partial class ImportPayeeSelection
    {
        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public int ImportRegexId { get; set; }
        public string ImportedCaption { get; set; }
        public int PayeeId { get; set; }
        public string TransactionCaption { get; set; }

        public virtual Category Category { get; set; }
        public virtual ImportRegex ImportRegex { get; set; }
        public virtual Payee Payee { get; set; }
    }
}
