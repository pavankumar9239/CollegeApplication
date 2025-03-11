﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Repository.DBContext;

#nullable disable

namespace Repository.Migrations
{
    [DbContext(typeof(CollegeDBContext))]
    [Migration("20250311122805_InitialStudenData")]
    partial class InitialStudenData
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Repository.Models.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DOB")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Students");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Address = "SKLM",
                            DOB = new DateTime(1997, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "apk@gmail.com",
                            Name = "APK"
                        },
                        new
                        {
                            Id = 2,
                            Address = "VIZAG",
                            DOB = new DateTime(2000, 9, 29, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "poojita@gmail.com",
                            Name = "Poojita"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
