using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WUCSA.Core.Entities.BlogModel;
using WUCSA.Core.Entities.GalleryModel;
using WUCSA.Core.Entities.StaffModel;
using WUCSA.Core.Entities.UserModel;

namespace WUCSA.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, UserRole, string>
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=WUCSA_DB;Trusted_Connection=True;MultipleActiveResultSets=true");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Blog>(entity =>
            {
                entity.HasMany(m => m.Comments)
                    .WithOne(m => m.Blog);

            });

            builder.Entity<Comment>(entity =>
            {
                entity.HasMany(m => m.Replies)
                    .WithOne(m => m.ParentComment);
            });

            builder.Entity<BlogTag>(entity =>
            {
                entity.HasKey(k => new { k.BlogId, k.TagId });

                entity.HasOne(m => m.Blog)
                    .WithMany(m => m.BlogTags)
                    .HasForeignKey(m => m.BlogId);

                entity.HasOne(m => m.Tag)
                    .WithMany(m => m.BlogTags)
                    .HasForeignKey(m => m.TagId);
            });

            builder.Entity<MediaTag>(entity =>
            {
                entity.HasKey(k => new { k.MediaId, k.MTagId });

                entity.HasOne(m => m.Media)
                    .WithMany(m => m.MediaTags)
                    .HasForeignKey(m => m.MediaId);

                entity.HasOne(m => m.MTag)
                    .WithMany(m => m.MediaTags)
                    .HasForeignKey(m => m.MTagId);

            });

            builder.Entity<Staff>(entity =>
            {
                entity.HasMany(m => m.Certificates)
                    .WithOne(m => m.Staff);

            });
        }
    }
}




