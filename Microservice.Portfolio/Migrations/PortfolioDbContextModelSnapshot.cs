﻿// <auto-generated />
using System;
using Microservice.Portfolio.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Microservice.Portfolio.Migrations
{
    [DbContext(typeof(PortfolioDbContext))]
    partial class PortfolioDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Microservice.Portfolio.DataAccess.Portfolio", b =>
                {
                    b.Property<Guid>("PortfolioId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("PortfolioName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("PortfolioId");

                    b.ToTable("portfolio", (string)null);
                });

            modelBuilder.Entity("Microservice.Portfolio.DataAccess.Portfolio+Asset", b =>
                {
                    b.Property<Guid>("AssetId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AssetName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<float>("AssetPrice")
                        .HasColumnType("real");

                    b.Property<Guid?>("PortfolioId")
                        .HasColumnType("uuid");

                    b.HasKey("AssetId");

                    b.HasIndex("PortfolioId");

                    b.ToTable("Asset");
                });

            modelBuilder.Entity("Microservice.Portfolio.DataAccess.Portfolio+Asset", b =>
                {
                    b.HasOne("Microservice.Portfolio.DataAccess.Portfolio", null)
                        .WithMany("Coin")
                        .HasForeignKey("PortfolioId");
                });

            modelBuilder.Entity("Microservice.Portfolio.DataAccess.Portfolio", b =>
                {
                    b.Navigation("Coin");
                });
#pragma warning restore 612, 618
        }
    }
}
