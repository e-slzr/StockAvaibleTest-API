using Microsoft.EntityFrameworkCore;
using StockAvaibleTest_API.Models;

namespace StockAvaibleTest_API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Box> Boxes { get; set; }
        public DbSet<BoxProductTransaction> BoxProductTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure unique constraints
            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Code)
                .IsUnique();

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Code)
                .IsUnique();

            modelBuilder.Entity<Box>()
                .HasIndex(b => b.Code)
                .IsUnique();

            // Configure check constraint for BoxProductTransaction
            modelBuilder.Entity<BoxProductTransaction>()
                .HasCheckConstraint("CK_BoxProductTransaction_Type", "Type IN ('IN', 'OUT')")
                .HasCheckConstraint("CK_BoxProductTransaction_Quantity", "Quantity > 0");
        }
    }
}
