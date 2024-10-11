using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QueueManagementSystemAPI.Models;

namespace QueueManagementSystemAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<App1User, AppRole, int, 
            IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>,
            IdentityUserToken<int>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //public virtual DbSet<Counter> Counters { get; set; }
        //public virtual DbSet<Service> Services {  get; set; }
        //public virtual DbSet<CounterService> CountersService { get; set; }
        //public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<Ticket> Ticket { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }

        public virtual DbSet<Project> Projects { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<App1User>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            builder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            builder.Entity<Ticket>()
                .HasMany(t => t.Comments)
                .WithOne(c => c.Ticket)
                .HasForeignKey(c => c.TicketId);
    
            builder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId);

            builder.Entity<Ticket>()
                .HasOne(t => t.CreatedBy)
                .WithMany()
                .HasForeignKey(t => t.CreatedById)
                .OnDelete(DeleteBehavior.Restrict); 

            builder.Entity<Ticket>()
                .HasOne(t => t.AssignedTo)
                .WithMany()
                .HasForeignKey(t => t.AssignedToId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Project>()
                .HasMany(p => p.Users)
                .WithOne(u => u.Project)
                .HasForeignKey(u => u.ProjectId);

            //builder.Entity<Counter>()
            //    .HasOne<Ticket>(z => z.TicketOrder)
            //    .WithOne(z => z.TicketCounter)
            //    .HasForeignKey<Counter>(z => z.TicketId)
            //    .OnDelete(DeleteBehavior.NoAction);

            //builder.Entity<CounterService>()
            //    .HasKey(z => new { z.ServiceId, z.CounterId });

            //builder.Entity<CounterService>()
            //    .HasOne(z => z.Service)
            //    .WithMany(s => s.ServicesOnCounter)
            //    .HasForeignKey(z => z.ServiceId);

            //builder.Entity<CounterService>()
            //    .HasOne(z => z.Counter)
            //    .WithMany(c => c.ServicesOnCounter)
            //    .HasForeignKey(z => z.CounterId);

            //builder.Entity<Ticket>()
            //    .HasOne(t => t.Service)
            //    .WithMany(s => s.Tickets)
            //    .HasForeignKey(t => t.ServiceId);

            //builder.Entity<Ticket>()
            //    .HasOne(t => t.App1User)
            //    .WithMany(u => u.Tickets)
            //    .HasForeignKey(t => t.Owner);

            //// Configure enum to be stored as int
            //builder.Entity<Ticket>()
            //    .Property(t => t.Status)
            //    .HasConversion<int>();

        }

    }
}
