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
            dtoObject.Account = this.Account.CreateDto<dto.Account>(db);
            dtoObject.Amount = new dto.CurrencyNumber { Currency = dtoObject.Account.Currency, Value = this.Amount };
            dtoObject.Caption = this.Caption;
            dtoObject.Date = this.Date;

            if(this.Category != null)
                dtoObject.Category = this.Category.CreateDto<dto.Category>(db);
            
            if(this.Payee != null)
                dtoObject.Payee = this.Payee.CreateDto<dto.Payee>(db);
                
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
            this.AccountId = dtoObject.Account.ID;

            this.Caption = dtoObject.Caption;
            this.Amount = dtoObject.Amount.Value;

            this.CategoryId = dtoObject.Category?.ID;

            this.PayeeId = dtoObject.Payee?.ID;

            this.Type = this.Type;
            this.UserDate = this.UserDate;

            this.ImportedTransactionCaption = dtoObject.ImportedTransactionCaption;
            this.ImportedTransactionHash = dtoObject.ImportedTransactionHash;
        }
    }
}
