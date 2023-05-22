﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyBlog.Data;

#nullable disable

namespace MyBlog.Data.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MyBlog.Data.EntityModels.Article", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("creation_date");

                    b.Property<string>("Header")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("header");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("image");

                    b.Property<string>("Introduction")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("introduction");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("last_mofified_date");

                    b.Property<int?>("ReadingTime")
                        .HasColumnType("int")
                        .HasColumnName("reading_time");

                    b.HasKey("Id");

                    b.ToTable("articles");
                });

            modelBuilder.Entity("MyBlog.Data.EntityModels.ContentBlock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ArticleId")
                        .HasColumnType("int")
                        .HasColumnName("article_id");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("content");

                    b.Property<int>("ContentType")
                        .HasColumnType("int")
                        .HasColumnName("content_type");

                    b.Property<int>("SerialNumber")
                        .HasColumnType("int")
                        .HasColumnName("serial_number");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.ToTable("content_blocks");
                });

            modelBuilder.Entity("MyBlog.Data.EntityModels.SelectedArticle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ArticleId")
                        .HasColumnType("int")
                        .HasColumnName("article_id");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2")
                        .HasColumnName("date");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.HasIndex("UserId");

                    b.ToTable("selected_articles");
                });

            modelBuilder.Entity("MyBlog.Data.EntityModels.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ArticleId")
                        .HasColumnType("int")
                        .HasColumnName("article_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.ToTable("tags");
                });

            modelBuilder.Entity("MyBlog.Data.EntityModels.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("email");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("password");

                    b.Property<Guid>("Salt")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("salt");

                    b.Property<int>("Status")
                        .HasColumnType("int")
                        .HasColumnName("status");

                    b.HasKey("Id");

                    b.ToTable("users");
                });

            modelBuilder.Entity("MyBlog.Data.EntityModels.ContentBlock", b =>
                {
                    b.HasOne("MyBlog.Data.EntityModels.Article", "Article")
                        .WithMany("Blocks")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Article");
                });

            modelBuilder.Entity("MyBlog.Data.EntityModels.SelectedArticle", b =>
                {
                    b.HasOne("MyBlog.Data.EntityModels.Article", "Article")
                        .WithMany()
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyBlog.Data.EntityModels.User", "User")
                        .WithMany("SelectedArticles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Article");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MyBlog.Data.EntityModels.Tag", b =>
                {
                    b.HasOne("MyBlog.Data.EntityModels.Article", "Article")
                        .WithMany("Tags")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Article");
                });

            modelBuilder.Entity("MyBlog.Data.EntityModels.Article", b =>
                {
                    b.Navigation("Blocks");

                    b.Navigation("Tags");
                });

            modelBuilder.Entity("MyBlog.Data.EntityModels.User", b =>
                {
                    b.Navigation("SelectedArticles");
                });
#pragma warning restore 612, 618
        }
    }
}