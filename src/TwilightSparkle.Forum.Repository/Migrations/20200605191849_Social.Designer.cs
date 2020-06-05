﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TwilightSparkle.Forum.Repository.DbContexts;

namespace TwilightSparkle.Forum.Repository.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20200605191849_Social")]
    partial class Social
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TwilightSparkle.Forum.DomainModel.Entities.Commentary", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CommentTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ThreadId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("ThreadId");

                    b.ToTable("Commentaries");
                });

            modelBuilder.Entity("TwilightSparkle.Forum.DomainModel.Entities.LikeDislike", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsLike")
                        .HasColumnType("bit");

                    b.Property<int>("ThreadId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ThreadId");

                    b.HasIndex("UserId");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("TwilightSparkle.Forum.DomainModel.Entities.Section", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Sections");
                });

            modelBuilder.Entity("TwilightSparkle.Forum.DomainModel.Entities.Thread", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SectionId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("SectionId");

                    b.ToTable("Threads");
                });

            modelBuilder.Entity("TwilightSparkle.Forum.DomainModel.Entities.UploadedImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ExternalId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FilePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MediaType")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("TwilightSparkle.Forum.DomainModel.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProfileImageId")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ProfileImageId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TwilightSparkle.Forum.DomainModel.Entities.Commentary", b =>
                {
                    b.HasOne("TwilightSparkle.Forum.DomainModel.Entities.User", "Author")
                        .WithMany("Commentaries")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TwilightSparkle.Forum.DomainModel.Entities.Thread", "Thread")
                        .WithMany("Commentaries")
                        .HasForeignKey("ThreadId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TwilightSparkle.Forum.DomainModel.Entities.LikeDislike", b =>
                {
                    b.HasOne("TwilightSparkle.Forum.DomainModel.Entities.Thread", "Thread")
                        .WithMany("Likes")
                        .HasForeignKey("ThreadId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TwilightSparkle.Forum.DomainModel.Entities.User", "User")
                        .WithMany("Likes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TwilightSparkle.Forum.DomainModel.Entities.Thread", b =>
                {
                    b.HasOne("TwilightSparkle.Forum.DomainModel.Entities.User", "Author")
                        .WithMany("Threads")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.ClientNoAction)
                        .IsRequired();

                    b.HasOne("TwilightSparkle.Forum.DomainModel.Entities.Section", "Section")
                        .WithMany()
                        .HasForeignKey("SectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TwilightSparkle.Forum.DomainModel.Entities.User", b =>
                {
                    b.HasOne("TwilightSparkle.Forum.DomainModel.Entities.UploadedImage", "ProfileImage")
                        .WithMany()
                        .HasForeignKey("ProfileImageId");
                });
#pragma warning restore 612, 618
        }
    }
}
