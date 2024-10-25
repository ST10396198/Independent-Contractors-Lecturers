using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using ContractMonthlyClaimSystem.Data;
using ContractMonthlyClaimSystem.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ContractMonthlyClaimSystem.Pages.Dashboard
{
    public class LecturerModel : PageModel
    {
        private readonly AppDbContext _context;

        // Property to hold the list of claims
        public List<MonthlyClaim> Claims { get; set; } = new List<MonthlyClaim>();

        // Constructor to inject the AppDbContext
        public LecturerModel(AppDbContext context)
        {
            _context = context;
        }

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

            // Fetch claims for the logged-in user
            var userId = HttpContext.Session.GetString("UserId"); // Assuming you store UserId in session
            Claims = _context.Claims
                .Where(c => c.UserId.ToString() == userId)
                .Include(c => c.SupportingDocuments) // Include supporting documents if needed
                .ToList();

            return Page();
        }
    }
}

