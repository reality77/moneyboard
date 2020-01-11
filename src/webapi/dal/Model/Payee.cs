using System;
using System.Collections.Generic;

namespace dal.Model
{
    public partial class Payee
    {
        public Payee()
        {
            ImportPayeeSelections = new HashSet<ImportPayeeSelection>();
            Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ImportPayeeSelection> ImportPayeeSelections { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
