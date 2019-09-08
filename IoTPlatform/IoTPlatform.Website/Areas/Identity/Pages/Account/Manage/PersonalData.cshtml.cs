using System.Threading.Tasks;
using IoTPlatform.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SmartBreadcrumbs.Attributes;

namespace IoTPlatform.Website.Areas.Identity.Pages.Account.Manage
{
    [Breadcrumb("Personal Data", FromPage = typeof(IndexModel))]
    public class PersonalDataModel : PageModel
    {
        private readonly ILogger<PersonalDataModel> _logger;
        private readonly UserManager<WebProfileUser> _userManager;

        public PersonalDataModel(
            UserManager<WebProfileUser> userManager,
            ILogger<PersonalDataModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            return Page();
        }
    }
}