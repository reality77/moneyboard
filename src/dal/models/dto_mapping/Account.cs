using System;
using System.Collections.Generic;
using System.Text;
using dto;

namespace dal.models
{
    public partial class Account : DalObject
    {
        public override IDtoObject LoadTo(IDtoObject dto, MoneyboardContext db)
        {
            var dtoObject = (dto.Account)dto;
            dtoObject.ID = this.ID;
            dtoObject.Name = this.Name;
            dtoObject.Currency = this.Currency;
            dtoObject.InitialBalance = new CurrencyNumber { Value = this.InitialBalance, Currency = this.Currency };
            dtoObject.Balance = new CurrencyNumber { Value = this.Balance, Currency = this.Currency };

            return dtoObject;
        }

        public override void UpdateFrom(IDtoObject dto, MoneyboardContext db)
        {
            var dtoObject = (dto.Account)dto;
            this.ID = dtoObject.ID;
            this.Name = dtoObject.Name;
            this.InitialBalance = dtoObject.InitialBalance.Value;
            this.Balance = dtoObject.Balance.Value;
            this.Currency = dtoObject.Currency;
        }
    }
}