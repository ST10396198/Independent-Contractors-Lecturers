using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ContractMonthlyClaimSystem.Validation;

namespace ContractMonthlyClaimSystem.Models
{
	public class SubmitClaimViewModel
	{
		[BindProperty]
		[Required(ErrorMessage = "Please select a course.")]
		public string SelectedCourse { get; set; } = "";

		[BindProperty]
		[Required(ErrorMessage = "Please enter hours worked.")]
		[Range(0.01, double.MaxValue, ErrorMessage = "Please enter a valid number of hours.")]
		public decimal HoursWorked { get; set; } = 0;

		[BindProperty]
		[Required(ErrorMessage = "Please enter the hourly rate.")]
		[Range(0.01, double.MaxValue, ErrorMessage = "Please enter a valid hourly rate.")]
		public decimal HourlyRate { get; set; } = 0;

		public decimal TotalAmount => HoursWorked * HourlyRate;

		[BindProperty]
		[Required(ErrorMessage = "Please provide a description.")]
		[MaxLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
		public string Description { get; set; } = "";

		[BindProperty]
		[Required(ErrorMessage = "Please upload supporting documents.")]
		[DataType(DataType.Upload)]
		[MaxFileSize(10 * 1024 * 1024, ErrorMessage = "File size should not exceed 10 MB.")]
		[AllowedExtensions(new string[] { ".pdf", ".docx", ".xlsx" })] // Custom validation attribute for allowed extensions
		public IFormFileCollection SupportingDocuments { get; set; }

		public List<SelectListItem> Courses { get; set; } = new List<SelectListItem>
		{
			new SelectListItem { Value = "Course1", Text = "Course 1" },
			new SelectListItem { Value = "Course2", Text = "Course 2" },
			new SelectListItem { Value = "Course3", Text = "Course 3" }
		};
	}
}
