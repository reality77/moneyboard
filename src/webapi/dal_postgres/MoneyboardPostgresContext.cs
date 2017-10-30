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
  
    }
}