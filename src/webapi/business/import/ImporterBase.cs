using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Diagnostics;
using dto.import;
using System.IO;

namespace business.import
{
    public delegate IEnumerable<dal.models.Transaction> ExistingTransactionsFromHashDelegate(string hash);

    public abstract class ImporterBase
    {
        public event ExistingTransactionsFromHashDelegate OnFindDuplicates;

        public abstract ImportedAccount Import(Stream stream);

        protected IEnumerable<dal.models.Transaction> RaiseOnFindDuplicates(string hash)
        {
            return this.OnFindDuplicates?.Invoke(hash);
        }
    }


    public enum ImportFileTypes
    {
        Unknown,
        QIF,
        OFX,
    }
}