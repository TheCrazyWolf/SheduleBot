﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ShedulerBotSgk.ModelDB;

#nullable disable

namespace ShedulerBotSgk.Migrations
{
    [DbContext(typeof(DB))]
    partial class DBModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.0");

            modelBuilder.Entity("ShedulerBotSgk.ModelDB.CacheGroups", b =>
                {
                    b.Property<int?>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("name")
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.ToTable("CacheGroups");
                });

            modelBuilder.Entity("ShedulerBotSgk.ModelDB.CacheTeachers", b =>
                {
                    b.Property<string>("id")
                        .HasColumnType("TEXT");

                    b.Property<string>("name")
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.ToTable("CacheTeachers");
                });

            modelBuilder.Entity("ShedulerBotSgk.ModelDB.Setting", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("AdminID")
                        .HasColumnType("INTEGER");

                    b.Property<long>("IdGroup")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Timer")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("TypeBot")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("ShedulerBotSgk.ModelDB.Task", b =>
                {
                    b.Property<int>("IdTask")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long?>("PeerId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ResultText")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Settingid")
                        .HasColumnType("INTEGER");

                    b.Property<char?>("TypeTask")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("IdTask");

                    b.HasIndex("Settingid");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("ShedulerBotSgk.ModelDB.Task", b =>
                {
                    b.HasOne("ShedulerBotSgk.ModelDB.Setting", null)
                        .WithMany("Tasks")
                        .HasForeignKey("Settingid");
                });

            modelBuilder.Entity("ShedulerBotSgk.ModelDB.Setting", b =>
                {
                    b.Navigation("Tasks");
                });
#pragma warning restore 612, 618
        }
    }
}
