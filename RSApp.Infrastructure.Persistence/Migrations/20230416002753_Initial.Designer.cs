﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RSApp.Infrastructure.Persistence.Context;

#nullable disable

namespace RSApp.Infrastructure.Persistence.Migrations {
  [DbContext(typeof(RSAppContext))]
  [Migration("20230416002753_Initial")]
  partial class Initial {
    protected override void BuildTargetModel(ModelBuilder modelBuilder) {
#pragma warning disable 612, 618
      modelBuilder
          .HasAnnotation("ProductVersion", "6.0.15")
          .HasAnnotation("Relational:MaxIdentifierLength", 128);

      SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

      modelBuilder.Entity("RSApp.Core.Domain.Entities.Favorite", b => {
        b.Property<int>("Id")
            .ValueGeneratedOnAdd()
            .HasColumnType("int");

        SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

        b.Property<DateTime>("CreatedAt")
            .HasColumnType("datetime2");

        b.Property<DateTime>("LastModifiedAt")
            .HasColumnType("datetime2");

        b.Property<int>("PropertyId")
            .HasColumnType("int");

        b.Property<string>("UserId")
            .IsRequired()
            .HasColumnType("nvarchar(max)");

        b.HasKey("Id");

        b.HasIndex("PropertyId");

        b.ToTable("Favorites", ( string )null);
      });

      modelBuilder.Entity("RSApp.Core.Domain.Entities.Image", b => {
        b.Property<int>("Id")
            .ValueGeneratedOnAdd()
            .HasColumnType("int");

        SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

        b.Property<DateTime>("CreatedAt")
            .HasColumnType("datetime2");

        b.Property<string>("ImagePath")
            .IsRequired()
            .HasColumnType("nvarchar(max)");

        b.Property<DateTime>("LastModifiedAt")
            .HasColumnType("datetime2");

        b.Property<int>("PropertyId")
            .HasColumnType("int");

        b.HasKey("Id");

        b.HasIndex("PropertyId");

        b.ToTable("Images", ( string )null);
      });

      modelBuilder.Entity("RSApp.Core.Domain.Entities.Property", b => {
        b.Property<int>("Id")
            .ValueGeneratedOnAdd()
            .HasColumnType("int");

        SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

        b.Property<string>("Agent")
            .IsRequired()
            .HasColumnType("nvarchar(max)");

        b.Property<double>("Area")
            .HasColumnType("float");

        b.Property<int>("Bathrooms")
            .HasColumnType("int");

        b.Property<string>("Code")
            .IsRequired()
            .HasColumnType("nvarchar(max)");

        b.Property<DateTime>("CreatedAt")
            .HasColumnType("datetime2");

        b.Property<string>("Description")
            .IsRequired()
            .HasColumnType("nvarchar(max)");

        b.Property<DateTime>("LastModifiedAt")
            .HasColumnType("datetime2");

        b.Property<string>("Portrait")
            .HasColumnType("nvarchar(max)");

        b.Property<double>("Price")
            .HasColumnType("float");

        b.Property<int>("Rooms")
            .HasColumnType("int");

        b.Property<int>("SaleId")
            .HasColumnType("int");

        b.Property<int>("TypeId")
            .HasColumnType("int");

        b.HasKey("Id");

        b.HasIndex("SaleId");

        b.HasIndex("TypeId");

        b.ToTable("Properties", ( string )null);
      });

      modelBuilder.Entity("RSApp.Core.Domain.Entities.PropertyUpgrade", b => {
        b.Property<int>("Id")
            .ValueGeneratedOnAdd()
            .HasColumnType("int");

        SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

        b.Property<DateTime>("CreatedAt")
            .HasColumnType("datetime2");

        b.Property<DateTime>("LastModifiedAt")
            .HasColumnType("datetime2");

        b.Property<int>("PropertyId")
            .HasColumnType("int");

        b.Property<int>("UpgradeId")
            .HasColumnType("int");

        b.HasKey("Id");

        b.HasIndex("PropertyId");

        b.HasIndex("UpgradeId");

        b.ToTable("PropertyUpgrades", ( string )null);
      });

      modelBuilder.Entity("RSApp.Core.Domain.Entities.PropType", b => {
        b.Property<int>("Id")
            .ValueGeneratedOnAdd()
            .HasColumnType("int");

        SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

        b.Property<DateTime>("CreatedAt")
            .HasColumnType("datetime2");

        b.Property<string>("Description")
            .IsRequired()
            .HasColumnType("nvarchar(max)");

        b.Property<DateTime>("LastModifiedAt")
            .HasColumnType("datetime2");

        b.Property<string>("Name")
            .IsRequired()
            .HasColumnType("nvarchar(max)");

        b.HasKey("Id");

        b.ToTable("PropTypes", ( string )null);
      });

      modelBuilder.Entity("RSApp.Core.Domain.Entities.Sale", b => {
        b.Property<int>("Id")
            .ValueGeneratedOnAdd()
            .HasColumnType("int");

        SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

        b.Property<DateTime>("CreatedAt")
            .HasColumnType("datetime2");

        b.Property<string>("Description")
            .IsRequired()
            .HasColumnType("nvarchar(max)");

        b.Property<DateTime>("LastModifiedAt")
            .HasColumnType("datetime2");

        b.Property<string>("Name")
            .IsRequired()
            .HasColumnType("nvarchar(max)");

        b.HasKey("Id");

        b.ToTable("Sales", ( string )null);
      });

      modelBuilder.Entity("RSApp.Core.Domain.Entities.Upgrade", b => {
        b.Property<int>("Id")
            .ValueGeneratedOnAdd()
            .HasColumnType("int");

        SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

        b.Property<DateTime>("CreatedAt")
            .HasColumnType("datetime2");

        b.Property<string>("Description")
            .IsRequired()
            .HasColumnType("nvarchar(max)");

        b.Property<DateTime>("LastModifiedAt")
            .HasColumnType("datetime2");

        b.Property<string>("Name")
            .IsRequired()
            .HasColumnType("nvarchar(max)");

        b.HasKey("Id");

        b.ToTable("Upgrades", ( string )null);
      });

      modelBuilder.Entity("RSApp.Core.Domain.Entities.Favorite", b => {
        b.HasOne("RSApp.Core.Domain.Entities.Property", "Property")
            .WithMany("Favorites")
            .HasForeignKey("PropertyId")
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        b.Navigation("Property");
      });

      modelBuilder.Entity("RSApp.Core.Domain.Entities.Image", b => {
        b.HasOne("RSApp.Core.Domain.Entities.Property", "Property")
            .WithMany("Images")
            .HasForeignKey("PropertyId")
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        b.Navigation("Property");
      });

      modelBuilder.Entity("RSApp.Core.Domain.Entities.Property", b => {
        b.HasOne("RSApp.Core.Domain.Entities.Sale", "Sale")
            .WithMany("Properties")
            .HasForeignKey("SaleId")
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        b.HasOne("RSApp.Core.Domain.Entities.PropType", "Type")
            .WithMany("Properties")
            .HasForeignKey("TypeId")
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        b.Navigation("Sale");

        b.Navigation("Type");
      });

      modelBuilder.Entity("RSApp.Core.Domain.Entities.PropertyUpgrade", b => {
        b.HasOne("RSApp.Core.Domain.Entities.Property", "Property")
            .WithMany("PropertyUpgrades")
            .HasForeignKey("PropertyId")
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        b.HasOne("RSApp.Core.Domain.Entities.Upgrade", "Upgrade")
            .WithMany("PropertyUpgrades")
            .HasForeignKey("UpgradeId")
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        b.Navigation("Property");

        b.Navigation("Upgrade");
      });

      modelBuilder.Entity("RSApp.Core.Domain.Entities.Property", b => {
        b.Navigation("Favorites");

        b.Navigation("Images");

        b.Navigation("PropertyUpgrades");
      });

      modelBuilder.Entity("RSApp.Core.Domain.Entities.PropType", b => {
        b.Navigation("Properties");
      });

      modelBuilder.Entity("RSApp.Core.Domain.Entities.Sale", b => {
        b.Navigation("Properties");
      });

      modelBuilder.Entity("RSApp.Core.Domain.Entities.Upgrade", b => {
        b.Navigation("PropertyUpgrades");
      });
#pragma warning restore 612, 618
    }
  }
}
