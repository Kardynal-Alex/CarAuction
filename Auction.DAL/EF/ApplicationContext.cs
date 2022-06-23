using Auction.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Auction.DAL.EF
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> opt) : base(opt) { }
        public DbSet<Lot> Lots { get; set; }
        public DbSet<LotState> LotStates { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Images> Images { get; set; }
        public DbSet<AuthorDescription> AuthorDescriptions { get; set; }
        /// <summary>
        /// Fluent Api
        /// Set connection 0ne-to-many((many)Lot-to-(one)User)
        /// Set connection 0ne-to-one((one)Lot-to-(one)LotState)
        /// Set connection 0ne-to-one((one)Lot-to-(one)Images)
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<Lot>()
                        .HasOne(x => x.User)
                        .WithMany(x => x.Lots)
                        .HasForeignKey(x => x.UserId);
            modelBuilder.Entity<Lot>()
                        .HasOne(x => x.LotState)
                        .WithOne(x => x.Lot)
                        .HasForeignKey<LotState>(x => x.LotId)
                        .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Lot>()
                        .HasOne(x => x.Images)
                        .WithOne(x => x.Lot)
                        .HasForeignKey<Images>(x => x.Id);
        }
    }
}
