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
            var dtoAccount = (dto.Account)dto;
            dtoAccount.ID = this.ID;
            dtoAccount.Name = this.Name;
            dtoAccount.InitialBalance = new CurrencyNumber { Value = this.InitialBalance, Currency = this.Currency };
            dtoAccount.Balance = new CurrencyNumber { Value = this.Balance, Currency = this.Currency };

            return dtoAccount;
        }

        public override void UpdateFrom(IDtoObject dto, MoneyboardContext db)
        {
            var dtoAccount = (dto.Account)dto;
            this.ID = dtoAccount.ID;
            this.Name = dtoAccount.Name;
            this.InitialBalance = dtoAccount.InitialBalance.Value;
            this.Balance = dtoAccount.Balance.Value;
            this.Currency = dtoAccount.InitialBalance.Currency;
        }
    }
}