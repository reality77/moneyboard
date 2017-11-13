using dal.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace dal_postgres
{
    public class MoneyboardPostgresContext : dal.MoneyboardContext
    {
        public MoneyboardPostgresContext(DbContextOptions options)
            : base(options)
        { }
  
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            //builder.Ignore<dal.models.virtuals.VirtualMonthlyCategoryStat>(); // ignored in migrations (only used for GROUP BY)
        }
    }
}