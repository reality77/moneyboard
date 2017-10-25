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
        public MoneyboardContext(DbContextOptions options)
            : base(options)
        { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Payee> Payees { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public void SeedData()
        {
            this.Database.EnsureCreated();
            
            // Seeding for testing purposes currently
            if(!this.Accounts.Any())
            {
                var defaultAccount = new Account
                    {
                        Name = "Courant",
                        InitialBalance = 0,
                        Currency = dto.ECurrency.EUR,
                    };

                this.Accounts.AddRange(
                    defaultAccount,
                    new Account
                    {
                        Name = "Livret",
                        InitialBalance = 0,
                        Currency = dto.ECurrency.EUR,
                    }
                );

                this.SaveChanges();

                this.Transactions.AddRange(
                    new Transaction
                    {
                        Account = defaultAccount,
                        Amount = 1500,
                        Caption = "Salary",
                        UserDate = DateTime.Now.AddDays(-5)
                    },
                    new Transaction
                    {
                        Account = defaultAccount,
                        Amount = -150,
                        Caption = "Shopping",
                        UserDate = DateTime.Now.AddDays(-1)
                    }
                );

                this.SaveChanges();
            }
        }

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

        public Transaction GetTransaction(int id)
        {
            return this.Transactions.SingleOrDefault(a => a.ID == id);
        }
    }
}