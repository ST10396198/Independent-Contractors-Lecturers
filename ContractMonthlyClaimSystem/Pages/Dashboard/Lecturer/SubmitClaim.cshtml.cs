using Microsoft.AspNetCore.Authorization;
using ContractMonthlyClaimSystem.Data;
using ContractMonthlyClaimSystem.Models;
using ContractMonthlyClaimSystem.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContractMonthlyClaimSystem.Pages.Dashboard.Lecturer
{
	public class SubmitClaimModel : PageModel
	{
		private readonly AppDbContext _context;

		public SubmitClaimModel(AppDbContext context)
		{
			_context = context;
		}

		[BindProperty]
		public SubmitClaimViewModel ClaimViewModel { get; set; } = new SubmitClaimViewModel();

		public IActionResult OnGet()
		{
            var userRole = HttpContext.Session.GetString("UserRole");

            if (userRole == "Admin")
            {
                return RedirectToPage("/Dashboard/Admin");
            }
            else if (userRole != "Lecturer")
            {
                return RedirectToPage("/Users/Login");
            }

			// Populate courses or other necessary data
			ClaimViewModel.Courses = GetCourses();

			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				// Repopulate the courses if model validation fails
				ClaimViewModel.Courses = new List<SelectListItem>
				{
					new SelectListItem { Value = "Course1", Text = "Course 1" },
					new SelectListItem { Value = "Course2", Text = "Course 2" },
					new SelectListItem { Value = "Course3", Text = "Course 3" }
				};
				return Page();
			}

			var currentUser = _context.User.Find(GetUserId());

			// Create the MonthlyClaim object using the form data
			var claim = new MonthlyClaim
			{
				UserId = currentUser.Id,                     // Assuming you're getting the current user's ID
				Course = (Course)Enum.Parse(typeof(Course), ClaimViewModel.SelectedCourse),    // Selected course from the form
				HoursWorked = ClaimViewModel.HoursWorked,  // Hours worked from the form
				HourlyRate = ClaimViewModel.HourlyRate,    // Hourly rate from the form
				Description = ClaimViewModel.Description,  // Work description from the form
				SubmissionDate = DateTime.Now,             // The date the claim is submitted
				Status = Status.Pending                    // Default status as Pending
			};

			// Save the claim to the database
			_context.Claims.Add(claim);

			// Handle file uploads if any files are submitted
			if (ClaimViewModel.SupportingDocuments != null && ClaimViewModel.SupportingDocuments.Count > 0)
			{
				await _context.SaveChangesAsync();
				await UploadSupportingDocuments(claim.Id, ClaimViewModel.SupportingDocuments);
			}

			TempData["ModalPopUpHeading"] = "Successfully Uploaded";
			TempData["ModalPopUpMessage"] = "Your claim has been successfully uploaded and is pending approval by an admin.";

			// Redirect to success or confirmation page
			return RedirectToPage("/Dashboard");
		}


		private List<SelectListItem> GetCourses()
		{
			// Implement your logic to fetch the list of courses
			return new List<SelectListItem>
			{
				new SelectListItem { Value = "Course1", Text = "Course 1" },
					new SelectListItem { Value = "Course2", Text = "Course 2" },
					new SelectListItem { Value = "Course3", Text = "Course 3" }
			};
		}

		private int GetUserId()
		{
			string userIdString = HttpContext.Session.GetString("UserId");
			int userId = 1;

			if (!string.IsNullOrEmpty(userIdString))
				int.TryParse(userIdString, out userId);

			return userId;
		}

		private async Task UploadSupportingDocuments(int claimId, IFormFileCollection files)
		{
			// Loop through each uploaded file
			foreach (var file in files)
			{
				if (file.Length > 0)
				{
					// Define the directory where files will be stored
					var directoryPath = Path.Combine("wwwroot/uploads", claimId.ToString());

					// Ensure the directory exists
					if (!Directory.Exists(directoryPath))
					{
						Directory.CreateDirectory(directoryPath);
					}

					// Define the full file path
					var filePath = Path.Combine(directoryPath, file.FileName);

					// Save the file to the server
					using (var stream = new FileStream(filePath, FileMode.Create))
					{
						await file.CopyToAsync(stream);
					}

					// Save the file details to the database
					var supportingDocument = new SupportingDocument
					{
						ClaimId = claimId,
						FilePath = filePath,
						FileSize = (int)file.Length, // File size in bytes
						FileName = file.FileName     // Name of the file
					};

					_context.SupportingDocuments.Add(supportingDocument);  // Add to database
				}
			}

			// Commit the changes to the database
			await _context.SaveChangesAsync();
		}
	}
}

