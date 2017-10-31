using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using dto;

namespace dal.models
{
    public partial class Transaction : DalObject
    {
        public override IDtoObject LoadTo(IDtoObject dto, MoneyboardContext db)
        {
            var dtoObject = (dto.Transaction)dto;
            dtoObject.ID = this.ID;
            dtoObject.AccountId = this.Account.ID;
            dtoObject.AccountName = this.Account.Name;
            dtoObject.Amount = new dto.CurrencyNumber { Currency = this.Account.Currency, Value = this.Amount };
            dtoObject.Caption = this.Caption;
            dtoObject.Date = this.Date;

            if (this.Category != null)
            {
                dtoObject.CategoryId = this.Category.ID;
                dtoObject.CategoryName = this.Category.Name;
            }

            if (this.Payee != null)
            {
                dtoObject.PayeeId = this.Payee.ID;
                dtoObject.PayeeName = this.Payee.Name;
            }
            
            dtoObject.Type = this.Type;

            dtoObject.UserDate = this.UserDate;

            dtoObject.ImportedTransactionCaption = this.ImportedTransactionCaption;
            dtoObject.ImportedTransactionHash = this.ImportedTransactionHash;

            return dtoObject;
        }

        public override void UpdateFrom(IDtoObject dto, MoneyboardContext db)
        {
            var dtoObject = (dto.Transaction)dto;
            this.ID = dtoObject.ID;
            this.AccountId = dtoObject.AccountId;

            this.Caption = dtoObject.Caption;
            this.Amount = dtoObject.Amount.Value;

            if (dtoObject.CategoryId != null)
                this.CategoryId = dtoObject.CategoryId;

            if (dtoObject.PayeeId != null)
                this.PayeeId = dtoObject.PayeeId;

            this.Type = this.Type;
            this.UserDate = this.UserDate;

            this.ImportedTransactionCaption = dtoObject.ImportedTransactionCaption;
            this.ImportedTransactionHash = dtoObject.ImportedTransactionHash;
        }
    }
}
