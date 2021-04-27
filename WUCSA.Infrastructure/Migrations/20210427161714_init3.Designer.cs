﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WUCSA.Infrastructure.Data;

namespace WUCSA.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20210427161714_init3")]
    partial class init3
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(32)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(32)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(32)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(32)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserToken");
                });

            modelBuilder.Entity("WUCSA.Core.Entities.BlogModel.Blog", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(32)")
                        .HasMaxLength(32);

                    b.Property<string>("AuthorId")
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContentRu")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContentUz")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CoverPhotoPath")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<bool>("IsCommentsLocked")
                        .HasColumnType("bit");

                    b.Property<DateTime>("PostedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Slug")
                        .HasColumnType("nvarchar(80)")
                        .HasMaxLength(80);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(80)")
                        .HasMaxLength(80);

                    b.Property<string>("TitleRu")
                        .IsRequired()
                        .HasColumnType("nvarchar(80)")
                        .HasMaxLength(80);

                    b.Property<string>("TitleUz")
                        .IsRequired()
                        .HasColumnType("nvarchar(80)")
                        .HasMaxLength(80);

                    b.Property<int>("ViewCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Blog");
                });

            modelBuilder.Entity("WUCSA.Core.Entities.BlogModel.BlogTag", b =>
                {
                    b.Property<string>("BlogId")
                        .HasColumnType("nvarchar(32)")
                        .HasMaxLength(32);

                    b.Property<string>("TagId")
                        .HasColumnType("nvarchar(32)")
                        .HasMaxLength(32);

                    b.HasKey("BlogId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("BlogTag");
                });

            modelBuilder.Entity("WUCSA.Core.Entities.BlogModel.Comment", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(32)")
                        .HasMaxLength(32);

                    b.Property<string>("AuthorEmail")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("AuthorId")
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("AuthorName")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("BlogId")
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<bool>("IsLocked")
                        .HasColumnType("bit");

                    b.Property<string>("ParentCommentId")
                        .HasColumnType("nvarchar(32)");

                    b.Property<DateTime>("PostedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("BlogId");

                    b.HasIndex("ParentCommentId");

                    b.ToTable("Comment");
                });

            modelBuilder.Entity("WUCSA.Core.Entities.BlogModel.Tag", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(32)")
                        .HasMaxLength(32);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(40)")
                        .HasMaxLength(40);

                    b.HasKey("Id");

                    b.ToTable("Tag");
                });

            modelBuilder.Entity("WUCSA.Core.Entities.EventModel.Event", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(32)")
                        .HasMaxLength(32);

                    b.Property<string>("CompetitionType")
                        .IsRequired()
                        .HasColumnType("nvarchar(80)")
                        .HasMaxLength(80);

                    b.Property<string>("CoverPhotoPath")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(160)")
                        .HasMaxLength(160);

                    b.Property<string>("DescriptionRu")
                        .IsRequired()
                        .HasColumnType("nvarchar(160)")
                        .HasMaxLength(160);

                    b.Property<string>("DescriptionUz")
                        .IsRequired()
                        .HasColumnType("nvarchar(80)")
                        .HasMaxLength(80);

                    b.Property<DateTime>("EventDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("EventPartsFileUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(80)")
                        .HasMaxLength(80);

                    b.Property<DateTime>("PostedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("RulesFileUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SportTypeId")
                        .IsRequired()
                        .HasColumnType("nvarchar(32)");

                    b.HasKey("Id");

                    b.HasIndex("SportTypeId");

                    b.ToTable("Event");
                });

            modelBuilder.Entity("WUCSA.Core.Entities.EventModel.EventParticipant", b =>
                {
                    b.Property<string>("EventId")
                        .HasColumnType("nvarchar(32)")
                        .HasMaxLength(32);

                    b.Property<string>("ParticipantId")
                        .HasColumnType("nvarchar(32)")
                        .HasMaxLength(32);

                    b.HasKey("EventId", "ParticipantId");

                    b.HasIndex("ParticipantId");

                    b.ToTable("EventParticipant");
                });

            modelBuilder.Entity("WUCSA.Core.Entities.GalleryModel.MTag", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(32)")
                        .HasMaxLength(32);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(40)")
                        .HasMaxLength(40);

                    b.HasKey("Id");

                    b.ToTable("MTag");
                });

            modelBuilder.Entity("WUCSA.Core.Entities.GalleryModel.Media", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(32)")
                        .HasMaxLength(32);

                    b.Property<string>("AuthorId")
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("MediaPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PostedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Media");
                });

            modelBuilder.Entity("WUCSA.Core.Entities.GalleryModel.MediaTag", b =>
                {
                    b.Property<string>("MediaId")
                        .HasColumnType("nvarchar(32)")
                        .HasMaxLength(32);

                    b.Property<string>("MTagId")
                        .HasColumnType("nvarchar(32)")
                        .HasMaxLength(32);

                    b.HasKey("MediaId", "MTagId");

                    b.HasIndex("MTagId");

                    b.ToTable("MediaTag");
                });

            modelBuilder.Entity("WUCSA.Core.Entities.ParticipantModel.Certificate", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(32)")
                        .HasMaxLength(32);

                    b.Property<string>("CertName")
                        .HasColumnType("nvarchar(40)")
                        .HasMaxLength(40);

                    b.Property<string>("CertPath")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("ParticipantId")
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("StaffId")
                        .HasColumnType("nvarchar(32)");

                    b.HasKey("Id");

                    b.HasIndex("ParticipantId");

                    b.HasIndex("StaffId");

                    b.ToTable("Certificate");
                });

            modelBuilder.Entity("WUCSA.Core.Entities.ParticipantModel.Participant", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(32)")
                        .HasMaxLength(32);

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(400)")
                        .HasMaxLength(400);

                    b.Property<string>("DescriptionRu")
                        .HasColumnType("nvarchar(400)")
                        .HasMaxLength(400);

                    b.Property<string>("DescriptionUz")
                        .HasColumnType("nvarchar(400)")
                        .HasMaxLength(400);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(40)")
                        .HasMaxLength(40);

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(40)")
                        .HasMaxLength(40);

                    b.Property<string>("ProfilePhotoPath")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<int>("Weight")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Participant");
                });

            modelBuilder.Entity("WUCSA.Core.Entities.ParticipantModel.Staff", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(32)")
                        .HasMaxLength(32);

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(400)")
                        .HasMaxLength(400);

                    b.Property<string>("DescriptionRu")
                        .HasColumnType("nvarchar(400)")
                        .HasMaxLength(400);

                    b.Property<string>("DescriptionUz")
                        .HasColumnType("nvarchar(400)")
                        .HasMaxLength(400);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(40)")
                        .HasMaxLength(40);

                    b.Property<bool>("IsMember")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(40)")
                        .HasMaxLength(40);

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasColumnType("nvarchar(80)")
                        .HasMaxLength(80);

                    b.Property<string>("PositionRu")
                        .IsRequired()
                        .HasColumnType("nvarchar(80)")
                        .HasMaxLength(80);

                    b.Property<string>("PositionUz")
                        .IsRequired()
                        .HasColumnType("nvarchar(80)")
                        .HasMaxLength(80);

                    b.Property<string>("ProfilePhotoPath")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Staff");
                });

            modelBuilder.Entity("WUCSA.Core.Entities.RankModel.Rank", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(32)")
                        .HasMaxLength(32);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(400)")
                        .HasMaxLength(400);

                    b.Property<string>("DescriptionRu")
                        .HasColumnType("nvarchar(400)")
                        .HasMaxLength(400);

                    b.Property<string>("DescriptionUz")
                        .HasColumnType("nvarchar(400)")
                        .HasMaxLength(400);

                    b.Property<int>("RankLocation")
                        .HasColumnType("int");

                    b.Property<string>("RankPartsFileUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RankYear")
                        .HasColumnType("datetime2");

                    b.Property<string>("SportTypeId")
                        .IsRequired()
                        .HasColumnType("nvarchar(32)");

                    b.HasKey("Id");

                    b.HasIndex("SportTypeId");

                    b.ToTable("Rank");
                });

            modelBuilder.Entity("WUCSA.Core.Entities.RankModel.RankParticipant", b =>
                {
                    b.Property<string>("RankId")
                        .HasColumnType("nvarchar(32)")
                        .HasMaxLength(32);

                    b.Property<string>("ParticipantId")
                        .HasColumnType("nvarchar(32)")
                        .HasMaxLength(32);

                    b.Property<int>("PositionNumber")
                        .HasColumnType("int");

                    b.HasKey("RankId", "ParticipantId");

                    b.HasIndex("ParticipantId");

                    b.ToTable("RankParticipant");
                });

            modelBuilder.Entity("WUCSA.Core.Entities.RankModel.SportType", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(32)")
                        .HasMaxLength(32);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("NameRu")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("NameUz")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("SportType");
                });

            modelBuilder.Entity("WUCSA.Core.Entities.UserModel.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(32)")
                        .HasMaxLength(32);

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(40)")
                        .HasMaxLength(40);

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(40)")
                        .HasMaxLength(40);

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("ProfilePhotoPath")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WUCSA.Core.Entities.UserModel.UserRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(32)")
                        .HasMaxLength(32);

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("WUCSA.Core.Entities.UserModel.UserRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("WUCSA.Core.Entities.UserModel.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("WUCSA.Core.Entities.UserModel.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("WUCSA.Core.Entities.UserModel.UserRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WUCSA.Core.Entities.UserModel.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("WUCSA.Core.Entities.UserModel.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WUCSA.Core.Entities.BlogModel.Blog", b =>
                {
                    b.HasOne("WUCSA.Core.Entities.UserModel.AppUser", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId");
                });

            modelBuilder.Entity("WUCSA.Core.Entities.BlogModel.BlogTag", b =>
                {
                    b.HasOne("WUCSA.Core.Entities.BlogModel.Blog", "Blog")
                        .WithMany("BlogTags")
                        .HasForeignKey("BlogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WUCSA.Core.Entities.BlogModel.Tag", "Tag")
                        .WithMany("BlogTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WUCSA.Core.Entities.BlogModel.Comment", b =>
                {
                    b.HasOne("WUCSA.Core.Entities.UserModel.AppUser", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId");

                    b.HasOne("WUCSA.Core.Entities.BlogModel.Blog", "Blog")
                        .WithMany("Comments")
                        .HasForeignKey("BlogId");

                    b.HasOne("WUCSA.Core.Entities.BlogModel.Comment", "ParentComment")
                        .WithMany("Replies")
                        .HasForeignKey("ParentCommentId");
                });

            modelBuilder.Entity("WUCSA.Core.Entities.EventModel.Event", b =>
                {
                    b.HasOne("WUCSA.Core.Entities.RankModel.SportType", "SportType")
                        .WithMany("Events")
                        .HasForeignKey("SportTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WUCSA.Core.Entities.EventModel.EventParticipant", b =>
                {
                    b.HasOne("WUCSA.Core.Entities.EventModel.Event", "Event")
                        .WithMany("EventParticipants")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WUCSA.Core.Entities.ParticipantModel.Participant", "Participant")
                        .WithMany("EventParticipants")
                        .HasForeignKey("ParticipantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WUCSA.Core.Entities.GalleryModel.Media", b =>
                {
                    b.HasOne("WUCSA.Core.Entities.UserModel.AppUser", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId");
                });

            modelBuilder.Entity("WUCSA.Core.Entities.GalleryModel.MediaTag", b =>
                {
                    b.HasOne("WUCSA.Core.Entities.GalleryModel.MTag", "MTag")
                        .WithMany("MediaTags")
                        .HasForeignKey("MTagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WUCSA.Core.Entities.GalleryModel.Media", "Media")
                        .WithMany("MediaTags")
                        .HasForeignKey("MediaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WUCSA.Core.Entities.ParticipantModel.Certificate", b =>
                {
                    b.HasOne("WUCSA.Core.Entities.ParticipantModel.Participant", "Participant")
                        .WithMany("Certificates")
                        .HasForeignKey("ParticipantId");

                    b.HasOne("WUCSA.Core.Entities.ParticipantModel.Staff", "Staff")
                        .WithMany("Certificates")
                        .HasForeignKey("StaffId");
                });

            modelBuilder.Entity("WUCSA.Core.Entities.RankModel.Rank", b =>
                {
                    b.HasOne("WUCSA.Core.Entities.RankModel.SportType", "SportType")
                        .WithMany("Ranks")
                        .HasForeignKey("SportTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WUCSA.Core.Entities.RankModel.RankParticipant", b =>
                {
                    b.HasOne("WUCSA.Core.Entities.ParticipantModel.Participant", "Participant")
                        .WithMany("RankParticipants")
                        .HasForeignKey("ParticipantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WUCSA.Core.Entities.RankModel.Rank", "Rank")
                        .WithMany("RankParticipants")
                        .HasForeignKey("RankId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
