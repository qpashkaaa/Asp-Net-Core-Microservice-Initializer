﻿// <auto-generated />
using System;
using System.Collections.Generic;
using AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.Database.Sql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AspNetCoreMicroserviceInitializer.Examples.Migrations
{
    [DbContext(typeof(DummyDbContext))]
    partial class DummyDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("DummySchema")
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.Database.DummyModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.PrimitiveCollection<List<string>>("AdditionalMessages")
                        .HasColumnType("text[]");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastUpdateDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Message")
                        .HasColumnType("text");

                    b.Property<int>("Number")
                        .HasColumnType("integer");

                    b.Property<bool>("Status")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("DummyModels", "DummySchema");
                });
#pragma warning restore 612, 618
        }
    }
}
