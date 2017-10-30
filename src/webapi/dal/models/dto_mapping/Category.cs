using System;
using System.Collections.Generic;
using System.Text;
using dto;

namespace dal.models
{
    public partial class Category : DalObject
    {
        public override IDtoObject LoadTo(IDtoObject dto, MoneyboardContext db)
        {
            var dtoObject = (dto.Category)dto;
            dtoObject.ID = this.ID;
            dtoObject.Name = this.Name;

            return dtoObject;
        }

        public override void UpdateFrom(IDtoObject dto, MoneyboardContext db)
        {
            var dtoObject = (dto.Category)dto;
            this.ID = dtoObject.ID;
            this.Name = dtoObject.Name;
        }
    }
}
