using Microsoft.EntityFrameworkCore;
using PayrollApi.Models;

namespace PayrollApi.Data
{
    public class PayrollDbContext : DbContext
    {
        public PayrollDbContext(DbContextOptions<PayrollDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employees", "dbo");
                entity.HasKey(e => e.EmployeeID);
                entity.Property(e => e.EmployeeID).ValueGeneratedOnAdd();
                entity.Property(e => e.EmployeeNumber).HasMaxLength(20).IsRequired();
                entity.HasIndex(e => e.EmployeeNumber).IsUnique();
                entity.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.LastName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Email).HasMaxLength(255);
                entity.Property(e => e.WorkEmail).HasMaxLength(255);
            });
        }
    }
}
