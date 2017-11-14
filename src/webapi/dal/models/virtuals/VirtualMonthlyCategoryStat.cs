using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace dal.models.virtuals
{
    // Temporary until EFCore supports GroupBy (in EF Core 2.1 ?)
    public class VirtualMonthlyCategoryStat
    {
        // only necessary because mandatory for EntityFramework to run
        public int ID { get; set; }

        public int? CategoryId { get; set; }

        public string CategoryName { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public decimal Value { get; set; }
    }
}
