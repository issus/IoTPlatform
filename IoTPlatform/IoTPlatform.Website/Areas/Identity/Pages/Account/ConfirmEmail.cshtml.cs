using System;
using System.Threading.Tasks;
using IoTPlatform.Data.EntityFramework;
using IoTPlatform.Data.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IoTPlatform.Website.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly SignInManager<WebProfileUser> _signInManager;
        private readonly UserManager<WebProfileUser> _userManager;

        public ConfirmEmailModel(UserManager<WebProfileUser> userManager,
            ApplicationDbContext db, SignInManager<WebProfileUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
        }

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null) return RedirectToPage("/Index");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound($"Unable to load user with ID '{userId}'.");

            var result = await _userManager.ConfirmEmailAsync(user, code);

            user.DateEmailConfirmed = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                throw new InvalidOperationException($"Error confirming email for user with ID '{userId}':");

            await _signInManager.SignInAsync(user, false);

            return Page();
        }
    }
}