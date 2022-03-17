using Auction.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Auction.DAL.EF
{
    /// <summary>
    /// Main DB context in the application.
    /// </summary>
    public class ApplicationContext : IdentityDbContext<User>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> opt) : base(opt) { }
        /// <summary>
        /// Gets and sets <see cref="DbSet"/> of <see cref="Lot"/>entities.
        /// </summary>
        public DbSet<Lot> Lots { get; set; }
        /// <summary>
        /// Gets and sets <see cref="DbSet"/> of <see cref="LotState"/>entities.
        /// </summary>
        public DbSet<LotState> LotStates { get; set; }
        /// <summary>
        /// Gets and sets <see cref="DbSet"/> of <see cref="Comment"/>entities.
        /// </summary>
        public DbSet<Comment> Comments { get; set; }
        /// <summary>
        /// Gets and sets <see cref="DbSet"/> of <see cref="Favorite"/>entities.
        /// </summary>
        public DbSet<Favorite> Favorites { get; set; }
        /// <summary>
        /// Gets and sets <see cref="DbSet"/> of <see cref="Images"/>entities.
        /// </summary>
        public DbSet<Images> Images { get; set; }
        /// <summary>
        /// Gets and sets <see cref="DbSet"/> of <see cref="AuthorDescription"/>entities.
        /// </summary>
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
