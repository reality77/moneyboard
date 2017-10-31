using System;
using System.Collections.Generic;
using System.Linq;

namespace dto.import
{
    public class PayeeRuleRegistration
    {
        public int RegexId { get; set; }

        public string ImportedCaption { get; set; }

        public int PayeeId { get; set; }

        public int? CategoryId { get; set; }

        public string TransactionCaption { get; set; }
    }
}