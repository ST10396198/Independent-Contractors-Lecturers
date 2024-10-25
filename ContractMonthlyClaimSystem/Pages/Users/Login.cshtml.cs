using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using ContractMonthlyClaimSystem.Data;
using ContractMonthlyClaimSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ContractMonthlyClaimSystem.Models.Users
{
    [BindProperties]
    public class LoginModel : PageModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";

        public string successMessage = "";
        public string errorMessage = "";

        private readonly AppDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

        public LoginModel(AppDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            var userSessionVariables = new[]
            {
                HttpContext.Session.GetString("UserId"),
                HttpContext.Session.GetString("UserFirstName"),
                HttpContext.Session.GetString("UserLastName"),
                HttpContext.Session.GetString("UserEmail"),
                HttpContext.Session.GetString("UserRole"),
                HttpContext.Session.GetString("UserPasswordHash"),
            };

            foreach (var sessVar in userSessionVariables)
                if (string.IsNullOrEmpty(sessVar))
                    return;

            Response.Redirect("/Dashboard");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                errorMessage = "Login failed. Please check your input.";
                return Page();
            }

            // Validate the user's credentials
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == Email);
            if (user == null || !VerifyPassword(user.PasswordHash, Password))
            {
                errorMessage = "Invalid email or password.";
                return Page();
            }

            TempData["ModalPopUpHeading"] = "Login Notification";
            TempData["ModalPopUpMessage"] = "You have been logged in successfully.";

            // Set session variables
            HttpContext.Session.SetString("UserId", user.Id.ToString());
            HttpContext.Session.SetString("UserFirstName", user.FirstName);
            HttpContext.Session.SetString("UserLastName", user.LastName);
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserPasswordHash", user.PasswordHash);
            HttpContext.Session.SetString("UserRole", user.Role.ToString());

            // Clear the fields for security
            Email = "";
            Password = "";
            ModelState.Clear();

            // Redirect to the original page or dashboard
            var returnUrl = HttpContext.Request.Query["ReturnUrl"].ToString();
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl); // Redirect to the originally requested page
            }
            else
            {
                return RedirectToPage("/Dashboard"); // Fallback to the dashboard
            }
        }

        private bool VerifyPassword(string storedHash, string password)
        {
            var result = _passwordHasher.VerifyHashedPassword(new User(), storedHash, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}

