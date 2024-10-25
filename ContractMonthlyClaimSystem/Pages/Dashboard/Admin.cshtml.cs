using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using ContractMonthlyClaimSystem.Data;
using ContractMonthlyClaimSystem.Models;
using ContractMonthlyClaimSystem.Models.Enums;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ContractMonthlyClaimSystem.Pages.Dashboard
{
    public class AdminModel : PageModel
    {
        private readonly AppDbContext _context;

        // Property to hold the list of claims
        public List<MonthlyClaim> Claims { get; set; } = new List<MonthlyClaim>();

        public AdminModel(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(string searchTerm)
        {
            var userRole = HttpContext.Session.GetString("UserRole");

            if (userRole == "Lecturer")
            {
                return RedirectToPage("/Dashboard/Lecturer");
            }
            else if (userRole != "Admin")
            {
                return RedirectToPage("/Users/Login");
            }

            IQueryable<MonthlyClaim> query = _context.Claims
                .Include(c => c.User) // Include User details
                .Include(c => c.SupportingDocuments); // Include supporting documents if necessary

			if (!string.IsNullOrWhiteSpace(searchTerm))
			{
				Status? status = Enum.TryParse(searchTerm, true, out Status parsedStatus) ? parsedStatus : (Status?)null;

				query = query.Where(c =>
					c.User.FirstName.Contains(searchTerm) ||
					c.User.LastName.Contains(searchTerm) ||
					c.User.Email.Contains(searchTerm) ||
					c.Description.Contains(searchTerm) ||
					(status.HasValue && c.Status == status.Value)
				);
			}

            Claims = await query.ToListAsync();
            return Page();
        }
    }
}

