﻿// <auto-generated />
using System;
using APIServer.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace APIServer.Persistence.Migrations
{
    [DbContext(typeof(ApiDbContext))]
    partial class ApiDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasPostgresExtension("citext")
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("APIServer.Domain.Core.Models.WebHooks.WebHook", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ContentType")
                        .HasColumnType("text");

                    b.Property<int[]>("HookEvents")
                        .HasColumnType("integer[]");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("LastTrigger")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Secret")
                        .HasColumnType("text");

                    b.Property<string>("WebHookUrl")
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.ToTable("WebHooks");
                });

            modelBuilder.Entity("APIServer.Domain.Core.Models.WebHooks.WebHookHeader", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedTimestamp")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.Property<long>("WebHookID")
                        .HasColumnType("bigint");

                    b.HasKey("ID");

                    b.HasIndex("WebHookID");

                    b.ToTable("WebHookHeader");
                });

            modelBuilder.Entity("APIServer.Domain.Core.Models.WebHooks.WebHookRecord", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Exception")
                        .HasColumnType("text");

                    b.Property<string>("Guid")
                        .HasColumnType("text");

                    b.Property<int>("HookType")
                        .HasColumnType("integer");

                    b.Property<string>("RequestBody")
                        .HasColumnType("text");

                    b.Property<string>("RequestHeaders")
                        .HasColumnType("text");

                    b.Property<string>("ResponseBody")
                        .HasColumnType("text");

                    b.Property<int>("Result")
                        .HasColumnType("integer");

                    b.Property<int>("StatusCode")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp without time zone");

                    b.Property<long>("WebHookID")
                        .HasColumnType("bigint");

                    b.HasKey("ID");

                    b.HasIndex("WebHookID");

                    b.ToTable("WebHooksHistory");
                });

            modelBuilder.Entity("APIServer.Domain.Core.Models.WebHooks.WebHookHeader", b =>
                {
                    b.HasOne("APIServer.Domain.Core.Models.WebHooks.WebHook", "WebHook")
                        .WithMany("Headers")
                        .HasForeignKey("WebHookID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WebHook");
                });

            modelBuilder.Entity("APIServer.Domain.Core.Models.WebHooks.WebHookRecord", b =>
                {
                    b.HasOne("APIServer.Domain.Core.Models.WebHooks.WebHook", "WebHook")
                        .WithMany("Records")
                        .HasForeignKey("WebHookID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WebHook");
                });

            modelBuilder.Entity("APIServer.Domain.Core.Models.WebHooks.WebHook", b =>
                {
                    b.Navigation("Headers");

                    b.Navigation("Records");
                });
#pragma warning restore 612, 618
        }
    }
}
