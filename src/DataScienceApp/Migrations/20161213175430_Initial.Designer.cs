using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using DataScienceApp.Data;

namespace DataScienceApp.Migrations
{
    [DbContext(typeof(DataScienceAppContext))]
    [Migration("20161213175430_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DataScienceApp.Models.PriceEntry", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Close");

                    b.Property<DateTime>("Date");

                    b.Property<decimal>("High");

                    b.Property<decimal>("Low");

                    b.Property<decimal>("Open");

                    b.Property<int>("StockID");

                    b.Property<string>("Symbol")
                        .HasAnnotation("MaxLength", 4);

                    b.HasKey("ID");

                    b.HasIndex("StockID");

                    b.ToTable("PriceEntries");
                });

            modelBuilder.Entity("DataScienceApp.Models.Stock", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.HasKey("ID");

                    b.ToTable("Stocks");
                });

            modelBuilder.Entity("DataScienceApp.Models.PriceEntry", b =>
                {
                    b.HasOne("DataScienceApp.Models.Stock", "Stock")
                        .WithMany("Prices")
                        .HasForeignKey("StockID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
