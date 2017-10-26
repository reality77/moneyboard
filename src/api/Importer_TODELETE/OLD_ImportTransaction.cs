using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace api.Importer
{
    [DataContract]
    public class OLD_ImportTransaction
    {
        [IgnoreDataMember]
        public OLD_ImportAccount OwnerAccount { get; set; }

        [DataMember]
        public string CheckNumber { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public DateTime? UserDate { get; set; }

        [DataMember]
        public DateTime? PostedDate { get; set; }

        [DataMember]
        public DateTime? AvailableDate { get; set; }

        [DataMember]
        public string ImportID { get; set; }

        [DataMember]
        public string ImportName { get; set; }

        [DataMember]
        public string ImportMemo { get; set; }

        [DataMember]
        public string ImportCategory { get; set; }

        [DataMember]
        public string ImportTargetTransferAccount { get; set; }

        [DataMember]
        public List<OLD_ImportTransactionDetail> Details { get; set; }

        public OLD_ImportTransaction()
        {
            this.Details = new List<OLD_ImportTransactionDetail>();
        }

        /// <summary>
        /// Utilisé pour ne pas créer de transactions en double pour les virements
        /// </summary>
        /// <returns></returns>
        public string GetTransferUniqueIdentifierTargetAccount()
        {
            return string.Format("{0:yyyyMMdd}/{1}/{2}", this.UserDate, this.Amount, this.ImportTargetTransferAccount.ToLower());
        }

        public DateTime GetUserDate()
        {
            if (this.UserDate != null)
                return this.UserDate.Value;
            else if (this.PostedDate != null)
                return this.PostedDate.Value;
            else
                throw new Exception("No user date to provide");
        }

        /// <summary>
        /// Utilisé pour ne pas créer de transactions en double pour les virements
        /// </summary>
        /// <returns></returns>
        public string GetTransferUniqueIdentifier()
        {
            return string.Format("{0:yyyyMMdd}/{1}/{2}", this.UserDate, this.Amount, this.ImportTargetTransferAccount.ToLower());
        }
    }

    [DataContract]
    public class OLD_ImportTransactionDetail
    {
        [IgnoreDataMember]
        public OLD_ImportTransaction OwnerTransaction { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public string ImportName { get; set; }

        [DataMember]
        public string ImportMemo { get; set; }

        [DataMember]
        public string ImportCategory { get; set; }

        [DataMember]
        public string ImportTargetTransferAccount { get; set; }

        /// <summary>
        /// Utilisé pour ne pas créer de transactions en double pour les virements
        /// </summary>
        /// <returns></returns>
        public string GetTransferUniqueIdentifier()
        {
            return string.Format("{0:yyyyMMdd}/{1}/{2}", this.OwnerTransaction.UserDate, this.Amount, this.ImportTargetTransferAccount.ToLower());
        }
    }
}
