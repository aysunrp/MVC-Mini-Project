using MCV_Mini_Project.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MCV_Mini_Project.Filters
{
    public class AdminAreaAuthorizationFilter : IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var area = context.RouteData.Values["area"]?.ToString();
            if (!string.Equals(area, "Admin", StringComparison.OrdinalIgnoreCase))
                return;

            var user = context.HttpContext.User;
            if (user.Identity?.IsAuthenticated != true)
            {
                context.Result = new ChallengeResult();
                return;
            }

            if (user.FindFirst("email_confirmed")?.Value != "true")
            {
                context.Result = new RedirectToActionResult("EmailNotConfirmed", "Account", new { area = "" });
                return;
            }

            var controller = context.RouteData.Values["controller"]?.ToString();
            var auth = context.HttpContext.RequestServices.GetRequiredService<IAuthorizationService>();

            if (string.Equals(controller, "Role", StringComparison.OrdinalIgnoreCase))
            {
                var roleResult = await auth.AuthorizeAsync(user, Policies.SuperAdminOnly);
                if (!roleResult.Succeeded)
                    context.Result = new ForbidResult();
                return;
            }

            var panelResult = await auth.AuthorizeAsync(user, Policies.AdminPanel);
            if (!panelResult.Succeeded)
            {
                context.Result = new ForbidResult();
                return;
            }

            if (string.Equals(controller, "User", StringComparison.OrdinalIgnoreCase))
            {
                var usersResult = await auth.AuthorizeAsync(user, Policies.ManageUsers);
                if (!usersResult.Succeeded)
                    context.Result = new ForbidResult();
            }
        }
    }
}
