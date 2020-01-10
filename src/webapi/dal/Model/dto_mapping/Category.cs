using System;
using System.Collections.Generic;
using System.Text;
using dto;

namespace dal.Model
{
    public partial class Category : DalObject
    {
        public override IDtoObject LoadTo(IDtoObject dto, MoneyboardContext db)
        {
            var dtoObject = (dto.Category)dto;
            dtoObject.ID = this.Id;
            dtoObject.Name = this.Name;

            return dtoObject;
        }

        public override void UpdateFrom(IDtoObject dto, MoneyboardContext db)
        {
            var dtoObject = (dto.Category)dto;
            this.Id = dtoObject.ID;
            this.Name = dtoObject.Name;
        }
    }
}
