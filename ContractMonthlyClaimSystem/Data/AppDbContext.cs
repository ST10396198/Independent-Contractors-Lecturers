using ContractMonthlyClaimSystem.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Proxies;

namespace ContractMonthlyClaimSystem.Data;

public class AppDbContext : DbContext
{
	public DbSet<User> User { get; set; }
	public DbSet<MonthlyClaim> Claims { get; set; }
	public DbSet<SupportingDocument> SupportingDocuments { get; set; }

	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		if (!optionsBuilder.IsConfigured)
		{
			optionsBuilder.UseLazyLoadingProxies(); // Enable lazy loading
		}
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<MonthlyClaim>()
			.HasOne(c => c.User) // Configure the Claim to User relationship
			.WithMany() // User can have many Claims
			.HasForeignKey(c => c.UserId) // Specify the FK property
			.OnDelete(DeleteBehavior.Cascade); // Set the delete behavior

		modelBuilder.Entity<SupportingDocument>()
			.HasOne(sd => sd.MonthlyClaim) // Configure the SupportingDocument to MonthlyClaim relationship
			.WithMany(mc => mc.SupportingDocuments) // MonthlyClaim can have many SupportingDocuments
			.HasForeignKey(sd => sd.ClaimId) // Specify the FK property
			.OnDelete(DeleteBehavior.Cascade); // Set the delete behavior
	}
}
