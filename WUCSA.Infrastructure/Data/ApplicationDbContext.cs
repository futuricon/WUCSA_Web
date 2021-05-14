using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WUCSA.Core.Entities.BlogModel;
using WUCSA.Core.Entities.EventModel;
using WUCSA.Core.Entities.GalleryModel;
using WUCSA.Core.Entities.RankModel;
using WUCSA.Core.Entities.StaffModel;
using WUCSA.Core.Entities.UserModel;

namespace WUCSA.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, UserRole, string>
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=WUCSA_DB;Trusted_Connection=True;MultipleActiveResultSets=true")
                    .UseLazyLoadingProxies();
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>(entity => { entity.ToTable(name: "Users"); });
            builder.Entity<UserRole>(entity => { entity.ToTable(name: "Roles"); });
            builder.Entity<IdentityUserRole<string>>(entity => { entity.ToTable("UserRoles"); });
            builder.Entity<IdentityUserClaim<string>>(entity => { entity.ToTable("UserClaims"); });
            builder.Entity<IdentityUserLogin<string>>(entity => { entity.ToTable("UserLogins"); });
            builder.Entity<IdentityUserToken<string>>(entity => { entity.ToTable("UserToken"); });
            builder.Entity<IdentityRoleClaim<string>>(entity => { entity.ToTable("RoleClaims"); });

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

            builder.Entity<EventParticipant>(entity =>
            {
                entity.HasMany(m => m.Participants)
                    .WithOne(m => m.EventParticipant);

                entity.HasOne(m => m.Event)
                    .WithMany(m => m.EventParticipants);
            });

            builder.Entity<EventSportType>(entity =>
            {
                entity.HasKey(k => new { k.EventId, k.SportTypeId });

                entity.HasOne(m => m.Event)
                    .WithMany(m => m.EventSportTypes)
                    .HasForeignKey(m => m.EventId);

                entity.HasOne(m => m.SportType)
                    .WithMany(m => m.EventSportTypes)
                    .HasForeignKey(m => m.SportTypeId);
            });

            builder.Entity<RankParticipant>(entity =>
            {
                entity.HasMany(m => m.Participants)
                    .WithOne(m => m.RankParticipant);

                entity.HasOne(m => m.Rank)
                    .WithMany(m => m.RankParticipants);
            });

            builder.Entity<SportType>(entity =>
            {
                entity.HasMany(m => m.Ranks)
                    .WithOne(m => m.SportType);
            });
        }
    }
}




