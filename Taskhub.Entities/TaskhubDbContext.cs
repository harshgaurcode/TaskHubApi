using Microsoft.EntityFrameworkCore;

namespace Taskhub.Entities
{
    public class TaskhubDbContext:DbContext
    {
        public TaskhubDbContext(DbContextOptions<TaskhubDbContext> options):base(options)
        {
            
        }
        public DbSet<Users> Users { get; set; } 
        
        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<UserOfficalInformation> UserOfficalInformations { get; set; }

        public DbSet<Project> Projects{ get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<ProjectMembers> ProjectMembers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>()
                .HasIndex(u => u.Id)
                .IsUnique();

            // Composite index on Status and Priority in Tickets table
            modelBuilder.Entity<Ticket>()
                .HasIndex(t => new { t.Status, t.Priority });

            // Index on UserId in UserOfficialInformation table
            modelBuilder.Entity<UserOfficalInformation>()
                .HasIndex(uoi => uoi.UserId);

            // Index on UserId in ProjectMembers table
            modelBuilder.Entity<ProjectMembers>()
                .HasIndex(pm => pm.UserId);

            // Index on RoleId in ProjectMembers table
            modelBuilder.Entity<ProjectMembers>()
                .HasIndex(pm => pm.RoleId);

            // Composite index on UserId and RoleId in ProjectMembers table
            modelBuilder.Entity<ProjectMembers>()
                .HasIndex(pm => new { pm.UserId, pm.RoleId });
        }



    }
}
