﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using learnaid_backend.Data;

#nullable disable

namespace learnaid_backend.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230815142346_AgregarEntidadesEjercicios")]
    partial class AgregarEntidadesEjercicios
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("learnaid_backend.Core.Models.Adaptaciones", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Adaptacion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("EjercicioNoAdaptadoId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EjercicioNoAdaptadoId");

                    b.ToTable("Adaptaciones");
                });

            modelBuilder.Entity("learnaid_backend.Core.Models.EjercicioAdaptado", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Consigna")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ejercicio")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("EjercitacionAdaptadaId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EjercitacionAdaptadaId");

                    b.ToTable("EjercicioAdaptado");
                });

            modelBuilder.Entity("learnaid_backend.Core.Models.EjercicioNoAdaptado", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Consigna")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ejercicio")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("EjercitacionNoAdaptadaId")
                        .HasColumnType("int");

                    b.Property<string>("Texto")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EjercitacionNoAdaptadaId");

                    b.ToTable("EjercicioNoAdaptado");
                });

            modelBuilder.Entity("learnaid_backend.Core.Models.EjercitacionAdaptada", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("EjercicioOriginalId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EjercicioOriginalId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("EjercitacionAdaptada");
                });

            modelBuilder.Entity("learnaid_backend.Core.Models.EjercitacionNoAdaptada", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Edad")
                        .HasColumnType("int");

                    b.Property<string>("Idioma")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("EjercitacionNoAdaptada");
                });

            modelBuilder.Entity("learnaid_backend.Core.Models.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Apellido")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Contraseña")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Foto")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Profesion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Usuario");
                });

            modelBuilder.Entity("learnaid_backend.Core.Models.Adaptaciones", b =>
                {
                    b.HasOne("learnaid_backend.Core.Models.EjercicioNoAdaptado", null)
                        .WithMany("Adaptaciones")
                        .HasForeignKey("EjercicioNoAdaptadoId");
                });

            modelBuilder.Entity("learnaid_backend.Core.Models.EjercicioAdaptado", b =>
                {
                    b.HasOne("learnaid_backend.Core.Models.EjercitacionAdaptada", null)
                        .WithMany("Ejercicios")
                        .HasForeignKey("EjercitacionAdaptadaId");
                });

            modelBuilder.Entity("learnaid_backend.Core.Models.EjercicioNoAdaptado", b =>
                {
                    b.HasOne("learnaid_backend.Core.Models.EjercitacionNoAdaptada", null)
                        .WithMany("Ejercicios")
                        .HasForeignKey("EjercitacionNoAdaptadaId");
                });

            modelBuilder.Entity("learnaid_backend.Core.Models.EjercitacionAdaptada", b =>
                {
                    b.HasOne("learnaid_backend.Core.Models.EjercitacionNoAdaptada", "EjercicioOriginal")
                        .WithMany()
                        .HasForeignKey("EjercicioOriginalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("learnaid_backend.Core.Models.Usuario", null)
                        .WithMany("Ejercicios")
                        .HasForeignKey("UsuarioId");

                    b.Navigation("EjercicioOriginal");
                });

            modelBuilder.Entity("learnaid_backend.Core.Models.EjercicioNoAdaptado", b =>
                {
                    b.Navigation("Adaptaciones");
                });

            modelBuilder.Entity("learnaid_backend.Core.Models.EjercitacionAdaptada", b =>
                {
                    b.Navigation("Ejercicios");
                });

            modelBuilder.Entity("learnaid_backend.Core.Models.EjercitacionNoAdaptada", b =>
                {
                    b.Navigation("Ejercicios");
                });

            modelBuilder.Entity("learnaid_backend.Core.Models.Usuario", b =>
                {
                    b.Navigation("Ejercicios");
                });
#pragma warning restore 612, 618
        }
    }
}
