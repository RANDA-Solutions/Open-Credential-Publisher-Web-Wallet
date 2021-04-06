using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Services.Extensions;
using OpenCredentialPublisher.Services.Implementations;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace OpenCredentialPublisher.ClrWallet.Pages.Account.Manage
{
    public class ProfileImageModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ProfileImageService _profileImageService;
        private readonly ILogger<ProfileImageModel> _logger;

        public ProfileImageModel(
            UserManager<ApplicationUser> userManager,
            ProfileImageService profileImageService,
            ILogger<ProfileImageModel> logger)
        {
            _userManager = userManager;
            _profileImageService = profileImageService;
            _logger = logger;
        }

        public string CurrentImageUrl { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Please choose profile image")]
            [Display(Name = "Profile Picture")]
            public IFormFile ProfileImage { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            CurrentImageUrl = user.ProfileImageUrl;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                using var image = Image.Load(Input.ProfileImage.OpenReadStream());
                image.Mutate(x => x.Resize(150, 150));
                byte[] imageBytes;
                using (var ms = new MemoryStream())
                {
                    await image.SaveAsPngAsync(ms);
                    imageBytes = ms.ToArray();
                }

                var url = await _profileImageService.SaveImageToBlobAsync(User.UserId(), imageBytes);
                CurrentImageUrl = url;
                StatusMessage = "Profile image updated.";
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            StatusMessage = "There was a problem updating your profile image.";
            return Page();
        }
    }
}
