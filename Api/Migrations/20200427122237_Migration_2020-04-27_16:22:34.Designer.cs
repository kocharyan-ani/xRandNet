﻿// <auto-generated />
using System;
using Api.Database.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Api.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20200427122237_Migration_2020-04-27_16:22:34")]
    partial class Migration_20200427_162234
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Api.Database.Models.App", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("FileId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("ReleaseNotes")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Version")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("FileId");

                    b.ToTable("Apps");
                });

            modelBuilder.Entity("Api.Database.Models.Bug", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AppId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("ReportDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Reporter")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Summary")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("AppId");

                    b.ToTable("Bugs");
                });

            modelBuilder.Entity("Api.Database.Models.File", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<byte[]>("Data")
                        .HasColumnType("longblob");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("MimeType")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Files");

                    b.HasDiscriminator<string>("Discriminator").HasValue("File");
                });

            modelBuilder.Entity("Api.Database.Models.Info", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Info");
                });

            modelBuilder.Entity("Api.Database.Models.Link", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<string>("Url")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Links");
                });

            modelBuilder.Entity("Api.Database.Models.News", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("DatePosted")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Title")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("News");
                });

            modelBuilder.Entity("Api.Database.Models.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Email")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("FacebookUrl")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("FirstName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("LastName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("LinkedInUrl")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("People");
                });

            modelBuilder.Entity("Api.Database.Models.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("Api.Database.Models.Publication", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Authors")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("FileId")
                        .HasColumnType("int");

                    b.Property<string>("Journal")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Title")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("FileId");

                    b.ToTable("Publications");
                });

            modelBuilder.Entity("Api.Database.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("FirstName")
                        .HasColumnType("varchar(12) CHARACTER SET utf8mb4")
                        .HasMaxLength(12);

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("LastName")
                        .HasColumnType("varchar(12) CHARACTER SET utf8mb4")
                        .HasMaxLength(12);

                    b.Property<string>("Password")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Username")
                        .HasColumnType("varchar(12) CHARACTER SET utf8mb4")
                        .HasMaxLength(12);

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Api.Database.Models.AppFile", b =>
                {
                    b.HasBaseType("Api.Database.Models.File");

                    b.HasDiscriminator().HasValue("AppFile");
                });

            modelBuilder.Entity("Api.Database.Models.ManualFile", b =>
                {
                    b.HasBaseType("Api.Database.Models.File");

                    b.HasDiscriminator().HasValue("ManualFile");
                });

            modelBuilder.Entity("Api.Database.Models.App", b =>
                {
                    b.HasOne("Api.Database.Models.AppFile", "File")
                        .WithMany()
                        .HasForeignKey("FileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Api.Database.Models.Bug", b =>
                {
                    b.HasOne("Api.Database.Models.App", "App")
                        .WithMany("Bug")
                        .HasForeignKey("AppId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Api.Database.Models.Publication", b =>
                {
                    b.HasOne("Api.Database.Models.File", "File")
                        .WithMany()
                        .HasForeignKey("FileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
