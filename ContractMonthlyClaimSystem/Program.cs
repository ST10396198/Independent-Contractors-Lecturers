using ContractMonthlyClaimSystem.Models;
using ContractMonthlyClaimSystem.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using ContractMonthlyClaimSystem.Data;

namespace ContractMonthlyClaimSystem
{
	public class Program
	{
		public static void Main(String[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddRazorPages();

			// Register the AppDbContext with the MySQL configuration
			builder.Services.AddDbContext<AppDbContext>(options =>
					options.UseMySql("server=localhost;user=ryan;password=password;database=CMCS-PROG6021",
						new MySqlServerVersion(new Version(10, 4, 32))));

			// Add session services
			builder.Services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
				options.Cookie.HttpOnly = true; // Set cookie properties
				options.Cookie.IsEssential = false; // Make the cookie essential
			});

			// Configure authentication
			builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(options =>
			{
				options.LoginPath = "/Users/Login"; // Redirect here if not authenticated
				options.AccessDeniedPath = "/Account/AccessDenied"; // Redirect here for forbidden access
			});

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();
			app.UseSession();
			app.UseAuthentication();
			app.UseAuthorization();
			app.UseStaticFiles();
			app.UseMiddleware<CookieSessionMiddleware>();

			app.MapRazorPages();

			app.Run();
		}
	}
}
