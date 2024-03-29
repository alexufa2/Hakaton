﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TemplateApp.DAL;

namespace TemplateApp.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("TemplateApp.DAL.Entities.DataGovRuEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("identifier");

                    b.Property<string>("organization");

                    b.Property<string>("organization_name");

                    b.Property<string>("title");

                    b.Property<string>("topic");

                    b.HasKey("Id");

                    b.ToTable("DataGovRuEntries");
                });

            modelBuilder.Entity("TemplateApp.DAL.Entities.DataGovRuEntryRow", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("EntryId");

                    b.Property<string>("Row");

                    b.HasKey("Id");

                    b.HasIndex("EntryId");

                    b.ToTable("DataGovRuEntryRows");
                });

            modelBuilder.Entity("TemplateApp.DAL.Entities.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Body")
                        .IsRequired();

                    b.Property<bool>("IsHtml");

                    b.Property<string>("ReceiverEmail")
                        .IsRequired();

                    b.Property<int>("SendAttemptsLimit");

                    b.Property<DateTime>("SendingDate");

                    b.Property<int>("State");

                    b.Property<string>("Subject")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("TemplateApp.DAL.Entities.Street", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FullAddress");

                    b.Property<string>("StreetName");

                    b.HasKey("Id");

                    b.ToTable("Streets");
                });

            modelBuilder.Entity("TemplateApp.DAL.Entities.StreetJsonInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Json");

                    b.Property<int?>("StreetId");

                    b.HasKey("Id");

                    b.HasIndex("StreetId");

                    b.ToTable("StreetJsonInfos");
                });

            modelBuilder.Entity("TemplateApp.DAL.Users.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ActivationKey");

                    b.Property<string>("Email");

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("Name");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired();

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired();

                    b.Property<string>("Surname");

                    b.Property<string>("TimeZoneName")
                        .IsRequired();

                    b.Property<int>("TimeZoneOffsetMinutes");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TemplateApp.DAL.Entities.DataGovRuEntryRow", b =>
                {
                    b.HasOne("TemplateApp.DAL.Entities.DataGovRuEntry", "Entry")
                        .WithMany()
                        .HasForeignKey("EntryId");
                });

            modelBuilder.Entity("TemplateApp.DAL.Entities.StreetJsonInfo", b =>
                {
                    b.HasOne("TemplateApp.DAL.Entities.Street", "Street")
                        .WithMany()
                        .HasForeignKey("StreetId");
                });
#pragma warning restore 612, 618
        }
    }
}
