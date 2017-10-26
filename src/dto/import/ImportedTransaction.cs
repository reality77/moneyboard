using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace dto.import
{
    public class ImportedTransaction
    {
        public DateTime TransactionDate { get; set; }

        public string CaptionOrPayee { get; set; }

        public string Category { get; set; }

        public string TransferTarget { get; set; }

        public decimal Amount { get; set; }

        public string Memo { get; set; }

        public string Number { get; set; }


        public string TransactionHash { get; set; }

        public string Error { get; set; }

        public void GenerateHash(HashAlgorithm hashAlgorithm)
        {
            string transactionString = $"td:{this.TransactionDate}/cp:{this.CaptionOrPayee}/tt:{this.TransferTarget}/a:{this.Amount}/m:{this.Memo}/n:{this.Number}";
            var hash = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(transactionString));
			this.TransactionHash = Convert.ToBase64String(hash);
        }
    }
}
