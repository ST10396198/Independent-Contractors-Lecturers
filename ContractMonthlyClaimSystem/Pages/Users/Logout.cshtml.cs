using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContractMonthlyClaimSystem.Models.Users
{
	public class LogoutModel : PageModel
	{
		// This method handles the logout process
		public async Task<IActionResult> OnPostLogoutAsync()
		{
			// Clear the user's session
			HttpContext.Session.Clear();

			// Sign out the user
			await HttpContext.SignOutAsync();

			// Clear cookies
			Response.Cookies.Delete("UserId");
			Response.Cookies.Delete("UserFirstName");
			Response.Cookies.Delete("UserLastName");
			Response.Cookies.Delete("UserEmail");
			Response.Cookies.Delete("UserRole");

			// Set temporary data for a popup message on the next page
			TempData["ModalPopUpHeading"] = "Logout Notification";
			TempData["ModalPopUpMessage"] = "You have been logged out successfully.";

			return RedirectToPage("/Index");
		}
	}
}
