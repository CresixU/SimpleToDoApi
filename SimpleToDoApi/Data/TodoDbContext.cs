using Microsoft.EntityFrameworkCore;
using SimpleToDoApi.Models.Entities;

namespace SimpleToDoApi.Data
{
    public class TodoDbContext : DbContext
    {
        public DbSet<Todo> Todos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Todo>(builder =>
            {
                builder.Property(x => x.Id)
                    .IsRequired();

                builder.Property(x => x.Title)
                    .IsRequired()
                    .HasMaxLength(100);
            });
        }

        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            
        }
    }
}
