using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Practice5.CardService.Models;

namespace Practice5.CardService
{
    public class CardContext : DbContext
    {
        public CardContext(DbContextOptions<CardContext> options) : base(options)
        {
        }

        public DbSet<Card> Cards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Card>(builder =>
            {
                builder.HasKey(c => c.Id);
                builder.ToTable("Cards");
                builder.OwnsMany(c => c.Tags);
                builder.OwnsOne(c => c.Expire);
            });
        }
    }
    public class BloggingContextFactory : IDesignTimeDbContextFactory<CardContext>
    {
        public CardContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CardContext>();
            optionsBuilder.UseNpgsql("User ID=postgres;Password=admin;Host=locahost;Port=5432;Database=mydb;Pooling=true;Min Pool Size=0;Max Pool Size=100;Connection Lifetime=0;");
            return new CardContext(optionsBuilder.Options);
        }
    }

}