﻿// <auto-generated />
using Email.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using System;

namespace Email.EntityFramework.Migrations
{
    [DbContext(typeof(EmailContext))]
    [Migration("20180103211928_SuppliersNewFields")]
    partial class SuppliersNewFields
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("Email.EntityFramework.Models.Supplier", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("City");

                    b.Property<string>("Contacts");

                    b.Property<string>("Country");

                    b.Property<string>("DisplayName");

                    b.Property<string>("Emails");

                    b.Property<string>("FolderName");

                    b.Property<bool>("IsNew");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Suppliers");
                });
#pragma warning restore 612, 618
        }
    }
}