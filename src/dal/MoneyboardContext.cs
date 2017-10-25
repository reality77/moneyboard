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
                    new Transaction { Account = defaultAccount, Amount = 1650, Caption = "Salary", UserDate = DateTime.Parse("2017-01-02") },
                    new Transaction { Account = defaultAccount, Amount = -150, Caption = "Shopping",UserDate = DateTime.Parse("2017-01-03") },
                    new Transaction { Account = defaultAccount, Amount = -10, Caption = "T1",UserDate = DateTime.Parse("2017-01-04") },
                    new Transaction { Account = defaultAccount, Amount = -10, Caption = "T2",UserDate = DateTime.Parse("2017-01-04") },
                    new Transaction { Account = defaultAccount, Amount = -10, Caption = "T3",UserDate = DateTime.Parse("2017-01-04") },
                    new Transaction { Account = defaultAccount, Amount = -10, Caption = "T4",UserDate = DateTime.Parse("2017-01-04") },
                    new Transaction { Account = defaultAccount, Amount = -10, Caption = "T5",UserDate = DateTime.Parse("2017-01-04") },
                    new Transaction { Account = defaultAccount, Amount = -10, Caption = "T6",UserDate = DateTime.Parse("2017-01-04") },
                    new Transaction { Account = defaultAccount, Amount = -10, Caption = "T7",UserDate = DateTime.Parse("2017-01-04") },
                    new Transaction { Account = defaultAccount, Amount = -10, Caption = "T8",UserDate = DateTime.Parse("2017-01-04") },
                    new Transaction { Account = defaultAccount, Amount = -10, Caption = "T9",UserDate = DateTime.Parse("2017-01-04") },
                    new Transaction { Account = defaultAccount, Amount = -10, Caption = "T10",UserDate = DateTime.Parse("2017-01-04") }
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