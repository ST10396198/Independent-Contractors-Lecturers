using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContractMonthlyClaimSystem.Models
{
	public class SupportingDocument
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public int ClaimId { get; set; }

		[ForeignKey("ClaimId")]
		public virtual MonthlyClaim MonthlyClaim { get; set; } = null!;

		[Required]
		public string FileName { get; set; } = null!;

		[Required]
		public string FilePath { get; set; } = null!;

		[Required]
		[Range(1, 10 * 1024 * 1024, ErrorMessage = "File size must be between 1 byte and 10 MB.")]
		public int FileSize { get; set; }
	}

}
