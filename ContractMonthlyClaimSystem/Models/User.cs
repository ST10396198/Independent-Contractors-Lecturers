using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ContractMonthlyClaimSystem.Models.Enums;

namespace ContractMonthlyClaimSystem.Models
{
	// User table in DB
	public class User
	{
		[Key]
		public int Id { get; set; }

		[Required(ErrorMessage = "First name is required")]
		[StringLength(50)]
		public string FirstName { get; set; } = "";

		[Required(ErrorMessage = "Last name is required")]
		[StringLength(50)]
		public string LastName { get; set; } = "";

		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage = "Invalid email address")]
		public string Email { get; set; } = "";

		[Required]
		public Role Role { get; set; } = Role.Lecturer;

		public string PasswordHash { get; set; } = ""; // This property will be stored in the database

		[NotMapped]
		public string FullName => $"{FirstName} {LastName}";
	}
}

