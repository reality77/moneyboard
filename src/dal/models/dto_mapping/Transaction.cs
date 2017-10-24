﻿using System;
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
            dtoObject.Category = this.Category.CreateDto<dto.Category>(db);
            dtoObject.Payee = this.Payee.CreateDto<dto.Payee>(db);
            dtoObject.Type = this.Type;
            dtoObject.UserDate = this.UserDate;

            return dtoObject;
        }

        public override void UpdateFrom(IDtoObject dto, MoneyboardContext db)
        {
            var dtoObject = (dto.Transaction)dto;
            this.ID = dtoObject.ID;
            this.AccountID = dtoObject.Account.ID;

            this.Amount = dtoObject.Amount.Value;

            this.CategoryID = dtoObject.Category?.ID;

            this.PayeeID = dtoObject.Payee?.ID;

            this.Type = this.Type;
            this.UserDate = this.UserDate;
        }
    }
}
