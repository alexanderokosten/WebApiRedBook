﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApiRedBook.Model;

namespace WebApiRedBook.Migrations
{
    [DbContext(typeof(RedBookBaseContext))]
    [Migration("20210621185506_AddNewColumn")]
    partial class AddNewColumn
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WebApiRedBook.Model.ClassItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<int>("TypeId")
                        .HasColumnName("Type_Id")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TypeId");

                    b.ToTable("ClassItem");
                });

            modelBuilder.Entity("WebApiRedBook.Model.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Biology")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ClassItemId")
                        .HasColumnName("ClassItem_Id")
                        .HasColumnType("int");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LimitingFactors")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Number")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecurityMeasures")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Spread")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StatusId")
                        .HasColumnName("Status_Id")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClassItemId");

                    b.HasIndex("StatusId");

                    b.ToTable("Item");
                });

            modelBuilder.Entity("WebApiRedBook.Model.Kingdom", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.HasKey("Id");

                    b.ToTable("Kingdom");
                });

            modelBuilder.Entity("WebApiRedBook.Model.Status", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Status");
                });

            modelBuilder.Entity("WebApiRedBook.Model.Type", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("KingdomId")
                        .HasColumnName("Kingdom_Id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.HasKey("Id");

                    b.HasIndex("KingdomId");

                    b.ToTable("Type");
                });

            modelBuilder.Entity("WebApiRedBook.Model.ClassItem", b =>
                {
                    b.HasOne("WebApiRedBook.Model.Type", "Type")
                        .WithMany("ClassItem")
                        .HasForeignKey("TypeId")
                        .HasConstraintName("FK_ClassItem_Type")
                        .IsRequired();
                });

            modelBuilder.Entity("WebApiRedBook.Model.Item", b =>
                {
                    b.HasOne("WebApiRedBook.Model.ClassItem", "ClassItem")
                        .WithMany("Item")
                        .HasForeignKey("ClassItemId")
                        .HasConstraintName("FK_Item_ClassItem")
                        .IsRequired();

                    b.HasOne("WebApiRedBook.Model.Status", "Status")
                        .WithMany("Item")
                        .HasForeignKey("StatusId")
                        .HasConstraintName("FK_Item_Status")
                        .IsRequired();
                });

            modelBuilder.Entity("WebApiRedBook.Model.Type", b =>
                {
                    b.HasOne("WebApiRedBook.Model.Kingdom", "Kingdom")
                        .WithMany("Type")
                        .HasForeignKey("KingdomId")
                        .HasConstraintName("FK_Type_Kingdom")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
