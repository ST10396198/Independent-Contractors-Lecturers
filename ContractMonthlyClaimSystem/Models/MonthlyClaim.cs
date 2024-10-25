using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ContractMonthlyClaimSystem.Models.Enums;
using ContractMonthlyClaimSystem.Models;

public class MonthlyClaim
{
	[Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;

    [Required]
    public Status Status { get; set; } = Status.Pending;

    [Required]
    public DateTime SubmissionDate { get; set; } = DateTime.Now;

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Hours worked must be greater than zero.")]
    public decimal HoursWorked { get; set; } = 0.0m;

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Hourly rate must be greater than zero.")]
    public decimal HourlyRate { get; set; } = 0.0m;

    [NotMapped]
    public decimal TotalAmount => HoursWorked * HourlyRate;

    [MaxLength(500, ErrorMessage = "Description must be less than 500 characters.")]
    public string Description { get; set; } = null!;

    [Required]
    public Course Course { get; set; } = Course.Course1;	// Temp default value

    public virtual ICollection<SupportingDocument> SupportingDocuments { get; set; } = new List<SupportingDocument>();
}
