﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApp_OOP_Lr9.DataBase;

#nullable disable

namespace WebApp_OOP_Lr9.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20241126132232_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WebApp_OOP_Lr9.DataBase.FinancingOption", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Caption")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NewBuildingId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("NewBuildingId");

                    b.ToTable("FinancingOptions");
                });

            modelBuilder.Entity("WebApp_OOP_Lr9.DataBase.NewBuilding", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Caption")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("NewBuildings");
                });

            modelBuilder.Entity("WebApp_OOP_Lr9.DataBase.Property", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Area")
                        .HasColumnType("int");

                    b.Property<int>("CountRooms")
                        .HasColumnType("int");

                    b.Property<int>("Floor")
                        .HasColumnType("int");

                    b.Property<int>("NewBuildingId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("NewBuildingId");

                    b.ToTable("Properties");
                });

            modelBuilder.Entity("WebApp_OOP_Lr9.DataBase.FinancingOption", b =>
                {
                    b.HasOne("WebApp_OOP_Lr9.DataBase.NewBuilding", "building")
                        .WithMany()
                        .HasForeignKey("NewBuildingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("building");
                });

            modelBuilder.Entity("WebApp_OOP_Lr9.DataBase.Property", b =>
                {
                    b.HasOne("WebApp_OOP_Lr9.DataBase.NewBuilding", "building")
                        .WithMany()
                        .HasForeignKey("NewBuildingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("building");
                });
#pragma warning restore 612, 618
        }
    }
}
