namespace ContractMonthlyClaimSystem.Middleware
{
	public class CookieSessionMiddleware
	{
		private readonly RequestDelegate _next;

		public CookieSessionMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			// Check if the cookie exists
			if (context.Request.Cookies.TryGetValue("UserSession", out var userData))
			{
				// Set session variables based on cookie data
				context.Session.SetString("UserSessionData", userData);
			}

			// Call the next middleware in the pipeline
			await _next(context);
		}
	}
}
