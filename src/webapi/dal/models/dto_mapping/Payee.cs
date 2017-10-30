using System;
using System.Collections.Generic;
using System.Text;
using dto;

namespace dal.models
{
    public partial class Payee : DalObject
    {
        public override IDtoObject LoadTo(IDtoObject dto, MoneyboardContext db)
        {
            var dtoObject = (dto.Payee)dto;
            dtoObject.ID = this.ID;
            dtoObject.Name = this.Name;

            return dtoObject;
        }

        public override void UpdateFrom(IDtoObject dto, MoneyboardContext db)
        {
            var dtoObject = (dto.Payee)dto;
            this.ID = dtoObject.ID;
            this.Name = dtoObject.Name;
        }
    }
}
