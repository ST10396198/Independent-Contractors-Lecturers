using ContractMonthlyClaimSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContractMonthlyClaimSystem.Data;
using Microsoft.AspNetCore.Identity;

namespace ContractMonthlyClaimSystem.Pages.Users
{
	public class SignUpModel : PageModel
	{
		private readonly AppDbContext _dbContext;
		private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

		[BindProperty]
		public User NewUser { get; set; }

		[BindProperty]
		public string PlainTextPassword { get; set; } = "";
		[BindProperty]
		public string ConfirmPassword { get; set; } = "";

		public SignUpModel(AppDbContext dbContext)
		{
			_dbContext = dbContext;
			NewUser = new User();
		}

		public void OnGet()
		{
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			if (PlainTextPassword != ConfirmPassword)
			{
				ModelState.AddModelError("PlainTextPassword", "Passwords do not match.");
				return Page();
			}

			// Hash the password
			NewUser.PasswordHash = _passwordHasher.HashPassword(NewUser, PlainTextPassword);

			// Add the new user to the database
			_dbContext.User.Add(NewUser);
			await _dbContext.SaveChangesAsync();

			// Redirect to a confirmation or success page (optional)
			return RedirectToPage("/Users/SignUpSuccess");
		}
	}
}

