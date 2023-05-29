using Microsoft.EntityFrameworkCore;
using ReminderToEmail.Models;

namespace ReminderToEmail.Data
{
    public class ReminderContext:DbContext
    {
        public ReminderContext(DbContextOptions<ReminderContext> opt):base(opt){}

        
        public DbSet<Reminder> Reminders { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasIndex(x => x.email).IsUnique();

            modelBuilder.Entity<User>().HasMany(x => x.reminders)
                                        .WithOne(a => a.user)
                                        .HasForeignKey(q=>q.createBy)
                                        .HasPrincipalKey(y=>y.Id);

            modelBuilder.Entity<Reminder>().HasOne(x => x.user)
                                            .WithMany(a => a.reminders)
                                            .HasForeignKey(p => p.createBy)
                                            .HasPrincipalKey(z => z.Id);
        }
    }
}
