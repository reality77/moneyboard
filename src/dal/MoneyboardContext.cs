using dal.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace dal
{
    public class MoneyboardContext : DbContext
    {
        public MoneyboardContext(DbContextOptions<MoneyboardContext> options)
            : base(options)
        { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Payee> Payees { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public Account GetAccount(int id)
        {
            return this.Accounts.SingleOrDefault(a => a.ID == id);
        }

        public Category GetCategory(int id)
        {
            return this.Categories.SingleOrDefault(a => a.ID == id);
        }

        public Payee GetPayee(int id)
        {
            return this.Payees.SingleOrDefault(a => a.ID == id);
        }
    }
}