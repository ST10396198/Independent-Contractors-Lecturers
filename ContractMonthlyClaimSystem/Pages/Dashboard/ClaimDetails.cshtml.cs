using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContractMonthlyClaimSystem.Data;
using ContractMonthlyClaimSystem.Models;

namespace ContractMonthlyClaimSystem.Pages.Dashboard
{
    public class ClaimDetailsModel : PageModel
    {
        private readonly AppDbContext _context;

        // Property to hold the claim details
        public MonthlyClaim Claim { get; set; }

		public string LecturerFullName { get; set; }

        [BindProperty]
        public string UpdatedStatus { get; set; } // Bind the selected status from the form

        // Constructor to inject the AppDbContext
        public ClaimDetailsModel(AppDbContext context)
        {
            _context = context;
        }

		public IActionResult OnGet(int id)
		{
			// Fetch the claim by ID
			Claim = _context.Claims
				.Include(c => c.SupportingDocuments) // Include supporting documents if needed
				.FirstOrDefault(c => c.Id == id);

			if (Claim == null)
				return NotFound(); // Return a 404 if the claim is not found

			// Fetch the lecturer's full name if Claim.UserId exists
			var user = _context.User.FirstOrDefault(u => u.Id == Claim.UserId);
			LecturerFullName = user?.FullName ?? "Lecturer Not Found"; // Provide a default if user is null

			return Page();
		}


        public IActionResult OnPostUpdateStatus(int id)
        {
            // Fetch the claim by ID
            Claim = _context.Claims.FirstOrDefault(c => c.Id == id);

            if (Claim == null)
            {
                return NotFound();
            }

			// Update the status of the claim
			// Try to parse the string UpdatedStatus into the Status enum
			if (Enum.TryParse(UpdatedStatus, out ContractMonthlyClaimSystem.Models.Enums.Status parsedStatus))
			{
				Claim.Status = parsedStatus;
				_context.SaveChanges(); // Save changes to the database
			}
			else
			{
				// Handle the case where parsing fails, e.g., invalid status
				ModelState.AddModelError(string.Empty, "Invalid status value");
				return Page();
			}
            _context.SaveChanges(); // Save changes to the database

			TempData["ModalPopUpHeading"] = "Claim status update.";
			TempData["ModalPopUpMessage"] = "The claim's status has been successfully updated.";
			TempData["ModalPopUpUrlRedirect"] = "/Dashboard";
			TempData["ModalPopUpUrlBtnText"] = "Back to Dashboard";
            return Page();
        }
    }
}

