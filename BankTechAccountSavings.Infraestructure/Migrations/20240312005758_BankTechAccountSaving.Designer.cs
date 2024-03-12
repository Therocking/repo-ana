﻿// <auto-generated />
using System;
using BankTechAccountSavings.Infraestructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BankTechAccountSavings.Infraestructure.Migrations
{
    [DbContext(typeof(AccountSavingDbContext))]
    [Migration("20240312005758_BankTechAccountSaving")]
    partial class BankTechAccountSaving
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BankTechAccountSavings.Domain.Entities.AccountSaving", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AccountName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("AccountNumber")
                        .HasColumnType("bigint");

                    b.Property<int>("AccountStatus")
                        .HasColumnType("int");

                    b.Property<int>("AccountType")
                        .HasColumnType("int");

                    b.Property<decimal>("AnnualInterestProjected")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("AnnualInterestRate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Bank")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("Currency")
                        .HasColumnType("int");

                    b.Property<decimal>("CurrentBalance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("DateClosed")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateOpened")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("DeletedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<decimal>("MonthlyInterestGenerated")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("MonthlyInterestRate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("UpdatedDate")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.ToTable("AccountSavings");
                });

            modelBuilder.Entity("BankTechAccountSavings.Domain.Entities.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ConfirmationNumber")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("DeletedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("nvarchar(13)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("TransactionStatus")
                        .HasColumnType("int");

                    b.Property<int>("TransactionType")
                        .HasColumnType("int");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("UpdatedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<long>("Voucher")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Transactions");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Transaction");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("BankTechAccountSavings.Domain.Entities.Deposit", b =>
                {
                    b.HasBaseType("BankTechAccountSavings.Domain.Entities.Transaction");

                    b.Property<decimal>("Credit")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("DestinationProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("DestinationProductNumber")
                        .HasColumnType("bigint");

                    b.HasIndex("DestinationProductId");

                    b.HasDiscriminator().HasValue("Deposit");
                });

            modelBuilder.Entity("BankTechAccountSavings.Domain.Entities.Transfer", b =>
                {
                    b.HasBaseType("BankTechAccountSavings.Domain.Entities.Transaction");

                    b.Property<int>("Commission")
                        .HasColumnType("int");

                    b.Property<decimal>("Credit")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Debit")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("DestinationProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("DestinationProductNumber")
                        .HasColumnType("bigint");

                    b.Property<Guid>("SourceProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("SourceProductNumber")
                        .HasColumnType("bigint");

                    b.Property<decimal>("Tax")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Total")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("TransferType")
                        .HasColumnType("int");

                    b.HasIndex("DestinationProductId");

                    b.HasIndex("SourceProductId");

                    b.ToTable("Transactions", t =>
                        {
                            t.Property("Credit")
                                .HasColumnName("Transfer_Credit");

                            t.Property("DestinationProductId")
                                .HasColumnName("Transfer_DestinationProductId");

                            t.Property("DestinationProductNumber")
                                .HasColumnName("Transfer_DestinationProductNumber");
                        });

                    b.HasDiscriminator().HasValue("Transfer");
                });

            modelBuilder.Entity("BankTechAccountSavings.Domain.Entities.Withdraw", b =>
                {
                    b.HasBaseType("BankTechAccountSavings.Domain.Entities.Transaction");

                    b.Property<string>("AccountName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Debit")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("SourceProductId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("SourceProductNumber")
                        .HasColumnType("bigint");

                    b.Property<decimal>("Tax")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Total")
                        .HasColumnType("decimal(18,2)");

                    b.Property<long>("WithdrawCode")
                        .HasColumnType("bigint");

                    b.Property<long>("WithdrawPassword")
                        .HasColumnType("bigint");

                    b.HasIndex("SourceProductId");

                    b.ToTable("Transactions", t =>
                        {
                            t.Property("Debit")
                                .HasColumnName("Withdraw_Debit");

                            t.Property("SourceProductId")
                                .HasColumnName("Withdraw_SourceProductId");

                            t.Property("SourceProductNumber")
                                .HasColumnName("Withdraw_SourceProductNumber");

                            t.Property("Tax")
                                .HasColumnName("Withdraw_Tax");

                            t.Property("Total")
                                .HasColumnName("Withdraw_Total");
                        });

                    b.HasDiscriminator().HasValue("Withdraw");
                });

            modelBuilder.Entity("BankTechAccountSavings.Domain.Entities.Deposit", b =>
                {
                    b.HasOne("BankTechAccountSavings.Domain.Entities.AccountSaving", "DestinationProduct")
                        .WithMany("Deposits")
                        .HasForeignKey("DestinationProductId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("DestinationProduct");
                });

            modelBuilder.Entity("BankTechAccountSavings.Domain.Entities.Transfer", b =>
                {
                    b.HasOne("BankTechAccountSavings.Domain.Entities.AccountSaving", "DestinationProduct")
                        .WithMany("TransfersAsDestination")
                        .HasForeignKey("DestinationProductId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BankTechAccountSavings.Domain.Entities.AccountSaving", "SourceProduct")
                        .WithMany("TransfersAsSource")
                        .HasForeignKey("SourceProductId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("DestinationProduct");

                    b.Navigation("SourceProduct");
                });

            modelBuilder.Entity("BankTechAccountSavings.Domain.Entities.Withdraw", b =>
                {
                    b.HasOne("BankTechAccountSavings.Domain.Entities.AccountSaving", "SourceProduct")
                        .WithMany("WithDraws")
                        .HasForeignKey("SourceProductId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("SourceProduct");
                });

            modelBuilder.Entity("BankTechAccountSavings.Domain.Entities.AccountSaving", b =>
                {
                    b.Navigation("Deposits");

                    b.Navigation("TransfersAsDestination");

                    b.Navigation("TransfersAsSource");

                    b.Navigation("WithDraws");
                });
#pragma warning restore 612, 618
        }
    }
}
