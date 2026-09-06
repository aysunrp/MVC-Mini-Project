using MCV_Mini_Project.Constants;
using MCV_Mini_Project.Helpers;
using MCV_Mini_Project.Models;
using MCV_Mini_Project.Services.Interface;
using MCV_Mini_Project.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MCV_Mini_Project.Controllers
{
    public class AccountController : Controller
    {
        private const string GenericLoginError = "Invalid email or password.";

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IEmailService emailService,
            IConfiguration config,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _config = config;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            return View(new RegisterVM());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            if (!ModelState.IsValid)
                return View(model);

            var existing = await _userManager.FindByEmailAsync(model.Email);
            if (existing is not null)
            {
                ModelState.AddModelError(nameof(model.Email), "This email address is already registered.");
                return View(model);
            }

            var user = new AppUser
            {
                UserName = model.Email.Trim(),
                Email = model.Email.Trim(),
                FullName = model.FullName.Trim(),
                EmailConfirmed = false,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View(model);
            }

            await _userManager.AddToRoleAsync(user, AppRoles.Member);

            var (link, sent) = await IssueConfirmationEmailAsync(user);
            return RedirectToEmailConfirmation(user.Email, link, sent);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult RegisterConfirmation(string? email)
        {
            return RedirectToAction(nameof(EmailConfirmation), new { email });
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult EmailConfirmation(string? email)
        {
            ViewBag.Inbox = _config["Smtp:ConfirmationTo"]
                            ?? Environment.GetEnvironmentVariable("SMTP_CONFIRMATION_TO")
                            ?? "aysunrp@code.edu.az";

            return View(new EmailConfirmationVM
            {
                Email = email,
                ConfirmLink = TempData["ConfirmLink"] as string,
                EmailSent = TempData["EmailSent"]?.ToString() == "1"
            });
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToLocal(returnUrl);

            return View(new LoginVM { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToLocal(model.ReturnUrl);

            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
            {
                ModelState.AddModelError(string.Empty, GenericLoginError);
                return View(model);
            }

            var passwordOk = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!passwordOk)
            {
                await _userManager.AccessFailedAsync(user);
                ModelState.AddModelError(string.Empty, GenericLoginError);
                return View(model);
            }

            if (await _userManager.IsLockedOutAsync(user))
            {
                ModelState.AddModelError(string.Empty, "This account is locked. Please try again later.");
                return View(model);
            }

            if (!user.EmailConfirmed && RequireConfirmedEmail)
            {
                ModelState.AddModelError(string.Empty, "Please confirm your email address before signing in.");
                ViewBag.ShowResend = true;
                ViewBag.ResendEmail = user.Email;
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user, model.Password, model.RememberMe, lockoutOnFailure: true);

            if (result.Succeeded)
                return RedirectToLocal(model.ReturnUrl);

            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "This account is locked. Please try again later.");
                return View(model);
            }

            if (result.IsNotAllowed)
            {
                ModelState.AddModelError(string.Empty, "Please confirm your email address before signing in.");
                ViewBag.ShowResend = true;
                ViewBag.ResendEmail = user.Email;
                return View(model);
            }

            ModelState.AddModelError(string.Empty, GenericLoginError);
            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string? userId, string? token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
            {
                return View("EmailConfirmation", new EmailConfirmationVM
                {
                    Failed = true,
                    Message = "Invalid confirmation link."
                });
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return View("EmailConfirmation", new EmailConfirmationVM
                {
                    Failed = true,
                    Message = "Invalid confirmation link."
                });
            }

            if (user.EmailConfirmed)
            {
                return View("EmailConfirmation", new EmailConfirmationVM
                {
                    Email = user.Email,
                    Succeeded = true,
                    Message = "Your email is already confirmed. You can sign in."
                });
            }

            if (user.EmailConfirmTokenExpiresAt is null ||
                user.EmailConfirmTokenExpiresAt < DateTime.UtcNow ||
                !TokenHasher.Matches(token, user.EmailConfirmTokenHash))
            {
                return View("EmailConfirmation", new EmailConfirmationVM
                {
                    Email = user.Email,
                    Failed = true,
                    Message = "This confirmation link is invalid, expired, or has already been used."
                });
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return View("EmailConfirmation", new EmailConfirmationVM
                {
                    Email = user.Email,
                    Failed = true,
                    Message = "This confirmation link is invalid, expired, or has already been used."
                });
            }

            user.EmailConfirmTokenHash = null;
            user.EmailConfirmTokenExpiresAt = null;
            await _userManager.UpdateAsync(user);

            return View("EmailConfirmation", new EmailConfirmationVM
            {
                Email = user.Email,
                Succeeded = true,
                Message = "Email confirmed. You can now sign in."
            });
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResendConfirmation(string? email)
        {
            return View(new ResendConfirmationVM { Email = email ?? string.Empty });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResendConfirmation(ResendConfirmationVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null || user.EmailConfirmed)
            {
                TempData["Success"] = "If this email is registered and unconfirmed, a new confirmation link has been sent.";
                return RedirectToAction(nameof(ResendConfirmation));
            }

            var (link, sent) = await IssueConfirmationEmailAsync(user);
            return RedirectToEmailConfirmation(user.Email, link, sent);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult EmailNotConfirmed()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
                return RedirectToAction(nameof(Login));

            var roles = await _userManager.GetRolesAsync(user);
            return View(new ProfileVM
            {
                FullName = user.FullName,
                Email = user.Email ?? string.Empty,
                EmailConfirmed = user.EmailConfirmed,
                Roles = roles
            });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileVM model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
                return RedirectToAction(nameof(Login));

            model.Email = user.Email ?? string.Empty;
            model.EmailConfirmed = user.EmailConfirmed;
            model.Roles = await _userManager.GetRolesAsync(user);

            if (!user.EmailConfirmed)
            {
                ModelState.AddModelError(string.Empty, "Confirm your email before updating your profile.");
                return View(model);
            }

            if (!ModelState.IsValid)
                return View(model);

            user.FullName = model.FullName.Trim();
            var update = await _userManager.UpdateAsync(user);
            if (!update.Succeeded)
            {
                foreach (var error in update.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View(model);
            }

            if (!string.IsNullOrWhiteSpace(model.NewPassword))
            {
                if (string.IsNullOrWhiteSpace(model.CurrentPassword))
                {
                    ModelState.AddModelError(nameof(model.CurrentPassword), "Current password is required to set a new password.");
                    return View(model);
                }

                var passwordResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (!passwordResult.Succeeded)
                {
                    foreach (var error in passwordResult.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                    return View(model);
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            TempData["Success"] = "Profile updated.";
            return RedirectToAction(nameof(Profile));
        }

        private bool RequireConfirmedEmail
        {
            get
            {
                var raw = _config["AUTH_REQUIRE_CONFIRMED_EMAIL"]
                          ?? Environment.GetEnvironmentVariable("AUTH_REQUIRE_CONFIRMED_EMAIL")
                          ?? "true";
                return !string.Equals(raw, "false", StringComparison.OrdinalIgnoreCase);
            }
        }

        private int ConfirmationHours
        {
            get
            {
                var raw = _config["EMAIL_CONFIRMATION_HOURS"]
                          ?? Environment.GetEnvironmentVariable("EMAIL_CONFIRMATION_HOURS");
                return int.TryParse(raw, out var hours) && hours > 0 ? hours : 24;
            }
        }

        private IActionResult RedirectToEmailConfirmation(string? email, string confirmLink, bool emailSent)
        {
            TempData["ConfirmLink"] = confirmLink;
            TempData["EmailSent"] = emailSent ? "1" : "0";
            return RedirectToAction(nameof(EmailConfirmation), new { email });
        }

        private async Task<(string Link, bool EmailSent)> IssueConfirmationEmailAsync(AppUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            user.EmailConfirmTokenHash = TokenHasher.Hash(token);
            user.EmailConfirmTokenExpiresAt = DateTime.UtcNow.AddHours(ConfirmationHours);
            await _userManager.UpdateAsync(user);

            var link = Url.Action(
                nameof(ConfirmEmail),
                "Account",
                new { area = "", userId = user.Id, token },
                Request.Scheme,
                Request.Host.Value) ?? string.Empty;

            if (string.IsNullOrWhiteSpace(link))
                return (string.Empty, false);

            if (!_emailService.IsConfigured)
            {
                _logger.LogWarning("SMTP is not fully configured. Confirmation page will show the confirm button.");
                return (link, false);
            }

            try
            {
                await _emailService.SendEmailConfirmationAsync(user.Email!, link);
                return (link, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send confirmation email.");
                return (link, false);
            }
        }

        private IActionResult RedirectToLocal(string? returnUrl)
        {
            var isStaff = User.IsInRole(AppRoles.SuperAdmin) || User.IsInRole(AppRoles.Admin);

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                if (!isStaff && returnUrl.Contains("/Admin", StringComparison.OrdinalIgnoreCase))
                    return RedirectToAction("AccessDenied", "Account");

                return Redirect(returnUrl);
            }

            if (isStaff)
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });

            return RedirectToAction("Index", "Home");
        }
    }
}
