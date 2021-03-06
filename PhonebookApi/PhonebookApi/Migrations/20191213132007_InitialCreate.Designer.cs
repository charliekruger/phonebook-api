﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PhonebookApi.DTO;

namespace PhonebookApi.Migrations
{
    [DbContext(typeof(PhonebookContext))]
    [Migration("20191213132007_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0");

            modelBuilder.Entity("PhonebookApi.DTO.ContactDetail", b =>
                {
                    b.Property<int>("ContactDetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Content")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<int>("PhonebookEntryId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("ContactDetailId");

                    b.HasIndex("PhonebookEntryId");

                    b.ToTable("ContactDetails");
                });

            modelBuilder.Entity("PhonebookApi.DTO.PhonebookEntry", b =>
                {
                    b.Property<int>("PhonebookEntryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Surname")
                        .HasColumnType("TEXT");

                    b.HasKey("PhonebookEntryId");

                    b.ToTable("PhonebookEntries");
                });

            modelBuilder.Entity("PhonebookApi.DTO.ContactDetail", b =>
                {
                    b.HasOne("PhonebookApi.DTO.PhonebookEntry", "PhonebookEntry")
                        .WithMany("ContactDetails")
                        .HasForeignKey("PhonebookEntryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
