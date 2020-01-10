using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace dal.Model
{
    public partial class MoneyboardContext : DbContext
    {
        IConfiguration _config;

        public MoneyboardContext()
        {
        }

        public MoneyboardContext(DbContextOptions<MoneyboardContext> options, IConfiguration config)
            : base(options)
        {
            _config = config;
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<ImportPayeeSelection> ImportPayeeSelections { get; set; }
        public virtual DbSet<ImportRegex> ImportRegexes { get; set; }
        public virtual DbSet<Payee> Payees { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(_config.GetConnectionString("Moneyboard"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Balance).HasColumnType("numeric");

                entity.Property(e => e.InitialBalance).HasColumnType("numeric");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<ImportPayeeSelection>(entity =>
            {
                entity.HasIndex(e => e.CategoryId);

                entity.HasIndex(e => e.ImportRegexId);

                entity.HasIndex(e => e.PayeeId);

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ImportedCaption).IsRequired();

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.ImportPayeeSelections)
                    .HasForeignKey(d => d.CategoryId);

                entity.HasOne(d => d.ImportRegex)
                    .WithMany(p => p.ImportPayeeSelections)
                    .HasForeignKey(d => d.ImportRegexId);

                entity.HasOne(d => d.Payee)
                    .WithMany(p => p.ImportPayeeSelections)
                    .HasForeignKey(d => d.PayeeId);
            });

            modelBuilder.Entity<ImportRegex>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.RegexString).IsRequired();
            });

            modelBuilder.Entity<Payee>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasIndex(e => e.AccountId);

                entity.HasIndex(e => e.CategoryId);

                entity.HasIndex(e => e.Date);

                entity.HasIndex(e => e.ImportedTransactionHash);

                entity.HasIndex(e => e.PayeeId);

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Amount).HasColumnType("numeric");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.AccountId);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.CategoryId);

                entity.HasOne(d => d.Payee)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.PayeeId);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    
    
        public void SeedData()
        {
            this.Database.EnsureCreated();
            
            // Seeding for testing purposes currently
            if(!this.Accounts.Any())
            {
                /*var defaultAccount = new Account
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

                this.SaveChanges();*/
            }

            // --- Création des regex d'import
            if (this.ImportRegexes.Count() < 3)
            {
                AddImportRegex("^VIR (?'mode'(.*?)) (?'payee'(.*))$", dto.ETransactionType.Transfer);
                AddImportRegex("^PAIEMENT (?'mode'(.*?)) (?'user_date_FR'(.*?)) ((?'comment'(.*?)) |)(?'payee'(.*))$", dto.ETransactionType.Payment);
                AddImportRegex("^RETRAIT (?'mode'(.*?)) (?'user_date_FR'(.*?)) (?'comment'(.*))$", dto.ETransactionType.Withdrawal, "Retrait");
                AddImportRegex("^PRLV (?'mode'(.*?)) (?'payee'(.*))$", dto.ETransactionType.Debit);

                this.SaveChanges();
            }

            // Recalcul de la balance 
            bool saveNecessary = false;
            foreach (var account in this.Accounts)
            {
                if (RecomputeBalance(account))
                    saveNecessary = true;
            }

            if (saveNecessary)
                this.SaveChanges();
        }

        private ImportRegex AddImportRegex(string regex, dto.ETransactionType transactionType = dto.ETransactionType.Unknown, string defaultCaption = null)
        {
            var ir = new ImportRegex { RegexString = regex, TransactionType = transactionType/*TODO, DefaultCaption = defaultCaption*/ };
            this.ImportRegexes.Add(ir);

            return ir;
        }

        private void AddImportPayeeSelection(ImportRegex regex, string importedCaption, int payeeId, int categoryId)
        {
            this.ImportPayeeSelections.Add(new ImportPayeeSelection { ImportRegexId = regex.Id, ImportedCaption = importedCaption, PayeeId = payeeId, CategoryId = categoryId });
        }

        public Account GetAccount(int id)
        {
            return this.Accounts.SingleOrDefault(a => a.Id == id);
        }

        public Category GetCategory(int id)
        {
            return this.Categories.SingleOrDefault(a => a.Id == id);
        }

        public Payee GetPayee(int id)
        {
            return this.Payees.SingleOrDefault(a => a.Id == id);
        }

        public Transaction GetTransaction(int id)
        {
            return this.Transactions.SingleOrDefault(a => a.Id == id);
        }
        
        /// <summary>
        /// Recompute the account current balance from the transactions
        /// </summary>
        /// <param name="account">The account from which the balance will be computed</param>
        /// <returns>true if the balance has been changed (changes must then be saved)</returns>
        public bool RecomputeBalance(Account account)
        {
            this.Entry(account).Collection(a => a.Transactions).Load();

            var balance = account.InitialBalance + account.Transactions.Sum(a => a.Amount);
            if (account.Balance != balance)
            {
                account.Balance = balance;
                return true;
            }
            else
                return false;
        }
    }
}
