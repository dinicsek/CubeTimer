﻿// <auto-generated />
using System;
using CubeTimer.WebApi.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CubeTimer.WebApi.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231207203417_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CubeTimer.WebApi.Infrastructure.Models.Cube", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("CubeEvent")
                        .HasColumnType("integer");

                    b.Property<string>("CubeType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Cubes");
                });

            modelBuilder.Entity("CubeTimer.WebApi.Infrastructure.Models.Session", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("SessionName")
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("CubeTimer.WebApi.Infrastructure.Models.Solve", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("CubeId")
                        .HasColumnType("integer");

                    b.Property<string>("Scramble")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("SessionId")
                        .HasColumnType("integer");

                    b.Property<int?>("SolveModifier")
                        .HasColumnType("integer");

                    b.Property<int>("Time")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CubeId");

                    b.HasIndex("SessionId");

                    b.HasIndex("UserId");

                    b.ToTable("Solves");
                });

            modelBuilder.Entity("CubeTimer.WebApi.Infrastructure.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("EmailVerifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CubeTimer.WebApi.Infrastructure.Models.Cube", b =>
                {
                    b.HasOne("CubeTimer.WebApi.Infrastructure.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("CubeTimer.WebApi.Infrastructure.Models.Session", b =>
                {
                    b.HasOne("CubeTimer.WebApi.Infrastructure.Models.User", "User")
                        .WithMany("Sessions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("CubeTimer.WebApi.Infrastructure.Models.Solve", b =>
                {
                    b.HasOne("CubeTimer.WebApi.Infrastructure.Models.Cube", "Cube")
                        .WithMany("Solves")
                        .HasForeignKey("CubeId");

                    b.HasOne("CubeTimer.WebApi.Infrastructure.Models.Session", "Session")
                        .WithMany("Solves")
                        .HasForeignKey("SessionId");

                    b.HasOne("CubeTimer.WebApi.Infrastructure.Models.User", "User")
                        .WithMany("Solves")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cube");

                    b.Navigation("Session");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CubeTimer.WebApi.Infrastructure.Models.Cube", b =>
                {
                    b.Navigation("Solves");
                });

            modelBuilder.Entity("CubeTimer.WebApi.Infrastructure.Models.Session", b =>
                {
                    b.Navigation("Solves");
                });

            modelBuilder.Entity("CubeTimer.WebApi.Infrastructure.Models.User", b =>
                {
                    b.Navigation("Sessions");

                    b.Navigation("Solves");
                });
#pragma warning restore 612, 618
        }
    }
}