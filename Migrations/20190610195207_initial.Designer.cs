﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using QuizappNet.Models;

namespace QuizappNet.Migrations
{
    [DbContext(typeof(QuizAppContext))]
    [Migration("20190610195207_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("QuizappNet.Models.Group", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("QuizappNet.Models.GroupUser", b =>
                {
                    b.Property<long>("GroupId");

                    b.Property<long>("UserId");

                    b.HasKey("GroupId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("GroupUsers");
                });

            modelBuilder.Entity("QuizappNet.Models.Question", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Complexity");

                    b.Property<string>("QuestionText");

                    b.HasKey("Id");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("QuizappNet.Models.QuestionChoice", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ChoiceText");

                    b.Property<bool>("IsRight");

                    b.Property<long?>("QuestionId");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("QuestionChoices");
                });

            modelBuilder.Entity("QuizappNet.Models.Quiz", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("MinPercentage");

                    b.Property<string>("Name");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.ToTable("Quizzes");
                });

            modelBuilder.Entity("QuizappNet.Models.QuizQuestion", b =>
                {
                    b.Property<long>("QuizId");

                    b.Property<long>("QuestionId");

                    b.HasKey("QuizId", "QuestionId");

                    b.HasIndex("QuestionId");

                    b.ToTable("QuizQuestions");
                });

            modelBuilder.Entity("QuizappNet.Models.Result", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("QuizId");

                    b.Property<double>("Score");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.HasIndex("QuizId");

                    b.ToTable("Results");
                });

            modelBuilder.Entity("QuizappNet.Models.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("QuizappNet.Models.GroupUser", b =>
                {
                    b.HasOne("QuizappNet.Models.Group", "Group")
                        .WithMany("usersLinks")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("QuizappNet.Models.User", "User")
                        .WithMany("groupsLinks")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("QuizappNet.Models.QuestionChoice", b =>
                {
                    b.HasOne("QuizappNet.Models.Question", "Question")
                        .WithMany("QuestionChoices")
                        .HasForeignKey("QuestionId");
                });

            modelBuilder.Entity("QuizappNet.Models.QuizQuestion", b =>
                {
                    b.HasOne("QuizappNet.Models.Question", "Question")
                        .WithMany("QuizzesLink")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("QuizappNet.Models.Quiz", "Quiz")
                        .WithMany("QuestionsLink")
                        .HasForeignKey("QuizId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("QuizappNet.Models.Result", b =>
                {
                    b.HasOne("QuizappNet.Models.Quiz", "Quiz")
                        .WithMany("Results")
                        .HasForeignKey("QuizId");
                });
#pragma warning restore 612, 618
        }
    }
}