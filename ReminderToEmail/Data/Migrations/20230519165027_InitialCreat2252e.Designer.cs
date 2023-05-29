﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ReminderToEmail.Data;

#nullable disable

namespace ReminderToEmail.Data.Migrations
{
    [DbContext(typeof(ReminderContext))]
    [Migration("20230519165027_InitialCreat2252e")]
    partial class InitialCreat2252e
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("ReminderToEmail.Models.Reminder", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("content")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("createAt")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("createBy")
                        .HasColumnType("TEXT");

                    b.Property<bool>("isSent")
                        .HasColumnType("INTEGER");

                    b.Property<string>("method")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("sendAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("to")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("createBy");

                    b.ToTable("Reminders");
                });

            modelBuilder.Entity("ReminderToEmail.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("createAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("password")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.HasKey("Id");

                    b.HasIndex("email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ReminderToEmail.Models.Reminder", b =>
                {
                    b.HasOne("ReminderToEmail.Models.User", "user")
                        .WithMany("reminders")
                        .HasForeignKey("createBy")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("user");
                });

            modelBuilder.Entity("ReminderToEmail.Models.User", b =>
                {
                    b.Navigation("reminders");
                });
#pragma warning restore 612, 618
        }
    }
}
