﻿// <auto-generated />
using dal_postgres;
using dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace dal_postgres.Migrations
{
    [DbContext(typeof(MoneyboardPostgresContext))]
    partial class MoneyboardPostgresContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

            modelBuilder.Entity("dal.models.Account", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Balance");

                    b.Property<int>("Currency");

                    b.Property<decimal>("InitialBalance");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("ID");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("dal.models.Category", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("ID");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("dal.models.Payee", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("ID");

                    b.ToTable("Payees");
                });

            modelBuilder.Entity("dal.models.Transaction", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccountID");

                    b.Property<decimal>("Amount");

                    b.Property<string>("Caption");

                    b.Property<int?>("CategoryID");

                    b.Property<int?>("PayeeID");

                    b.Property<int>("Type");

                    b.Property<DateTime>("UserDate");

                    b.HasKey("ID");

                    b.HasIndex("AccountID");

                    b.HasIndex("CategoryID");

                    b.HasIndex("PayeeID");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("dal.models.Transaction", b =>
                {
                    b.HasOne("dal.models.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("dal.models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryID");

                    b.HasOne("dal.models.Payee", "Payee")
                        .WithMany()
                        .HasForeignKey("PayeeID");
                });
#pragma warning restore 612, 618
        }
    }
}
