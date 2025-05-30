﻿// <auto-generated />
using System;
using System.Collections.Generic;
using GermanLearning.PostgreInfrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GermanLearning.PostgreInfrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250524112556_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ExerciseWord", b =>
                {
                    b.Property<Guid>("ExerciseId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("WordsId")
                        .HasColumnType("uuid");

                    b.HasKey("ExerciseId", "WordsId");

                    b.HasIndex("WordsId");

                    b.ToTable("ExerciseWord");
                });

            modelBuilder.Entity("GermanLearning.Domain.Entities.Exercise", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Difficulty")
                        .HasColumnType("integer");

                    b.Property<DateTime>("GeneratedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Exercises");
                });

            modelBuilder.Entity("GermanLearning.Domain.Entities.ExerciseResult", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CompletedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("CorrectAnswers")
                        .HasColumnType("integer");

                    b.Property<Guid>("ExerciseId")
                        .HasColumnType("uuid");

                    b.Property<int>("TotalQuestions")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ExerciseId");

                    b.ToTable("ExerciseResults");
                });

            modelBuilder.Entity("GermanLearning.Domain.Entities.Topic", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Topics");
                });

            modelBuilder.Entity("GermanLearning.Domain.Entities.Word", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.PrimitiveCollection<List<string>>("EnglishTranslation")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.PrimitiveCollection<List<string>>("ExampleSentences")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<int?>("Gender")
                        .HasColumnType("integer")
                        .HasColumnName("GenderId");

                    b.Property<int?>("GenderId")
                        .HasColumnType("integer");

                    b.Property<string>("GermanText")
                        .IsRequired()
                        .HasColumnType("text");

                    b.PrimitiveCollection<List<string>>("SpanishTranslation")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.PrimitiveCollection<List<string>>("Synonyms")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<int>("Type")
                        .HasColumnType("integer")
                        .HasColumnName("WordTypeId");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("WordTypeId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("GenderId");

                    b.HasIndex("WordTypeId");

                    b.ToTable("Words", t =>
                        {
                            t.Property("GenderId")
                                .HasColumnName("GenderId1");

                            t.Property("WordTypeId")
                                .HasColumnName("WordTypeId1");
                        });
                });

            modelBuilder.Entity("GermanLearning.PostgreInfrastructure.Contexts.GenderLookup", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("Genders", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 0,
                            Name = "None"
                        },
                        new
                        {
                            Id = 1,
                            Name = "Masculine"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Feminine"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Neutral"
                        });
                });

            modelBuilder.Entity("GermanLearning.PostgreInfrastructure.Contexts.WordTypeLookup", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("WordTypes", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 0,
                            Name = "None"
                        },
                        new
                        {
                            Id = 1,
                            Name = "Verb"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Noun"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Adjective"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Adverb"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Expression"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Other"
                        });
                });

            modelBuilder.Entity("WordTopic", b =>
                {
                    b.Property<Guid>("TopicsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("WordsId")
                        .HasColumnType("uuid");

                    b.HasKey("TopicsId", "WordsId");

                    b.HasIndex("WordsId");

                    b.ToTable("WordTopic");
                });

            modelBuilder.Entity("ExerciseWord", b =>
                {
                    b.HasOne("GermanLearning.Domain.Entities.Exercise", null)
                        .WithMany()
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GermanLearning.Domain.Entities.Word", null)
                        .WithMany()
                        .HasForeignKey("WordsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GermanLearning.Domain.Entities.ExerciseResult", b =>
                {
                    b.HasOne("GermanLearning.Domain.Entities.Exercise", "Exercise")
                        .WithMany()
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Exercise");
                });

            modelBuilder.Entity("GermanLearning.Domain.Entities.Word", b =>
                {
                    b.HasOne("GermanLearning.PostgreInfrastructure.Contexts.GenderLookup", null)
                        .WithMany()
                        .HasForeignKey("GenderId");

                    b.HasOne("GermanLearning.PostgreInfrastructure.Contexts.WordTypeLookup", null)
                        .WithMany()
                        .HasForeignKey("WordTypeId");
                });

            modelBuilder.Entity("WordTopic", b =>
                {
                    b.HasOne("GermanLearning.Domain.Entities.Topic", null)
                        .WithMany()
                        .HasForeignKey("TopicsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GermanLearning.Domain.Entities.Word", null)
                        .WithMany()
                        .HasForeignKey("WordsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
