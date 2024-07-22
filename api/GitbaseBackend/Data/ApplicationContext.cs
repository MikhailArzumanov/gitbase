using Microsoft.EntityFrameworkCore;
using GitbaseBackend.Models;

namespace GitbaseBackend.Data {
    public class ApplicationContext : DbContext {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) {
            //Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder.Entity<User>()
                .HasMany(u => u.OwnedRepositories)
                .WithOne(r => r.Owner)
                .HasForeignKey(r => r.OwnerId);
            modelBuilder.Entity<User>()
                .HasMany(u => u.CollaboratedRepositories)
                .WithMany(r => r.Collaborators);

            modelBuilder.Entity<User>()
                .HasMany(u => u.SshKeys)
                .WithOne(k => k.User)
                .HasForeignKey(k => k.UserId);
        }

        public DbSet<User>   Users   { get; set; }
        public DbSet<SshKey> SshKeys { get; set; }

        public DbSet<Repository> Repositories { get; set; }
    }
}
