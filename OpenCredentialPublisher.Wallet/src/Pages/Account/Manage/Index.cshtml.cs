using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenCredentialPublisher.Data.Contexts;
using OpenCredentialPublisher.Data.Models;

namespace OpenCredentialPublisher.ClrWallet.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public bool EmailIsConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name ="Username")]
            public string Username { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [MaxLength(255)]
            [Display(Name ="Displayable Name")]
            public string DisplayName { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            EmailIsConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            Input = new InputModel
            {
                Username = userName,
                PhoneNumber = phoneNumber,
                DisplayName = user.DisplayName
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            if (user.EmailConfirmed)
            {
                var username = await _userManager.GetUserNameAsync(user);
                if (String.IsNullOrWhiteSpace(Input.Username))
                {
                    ModelState.AddModelError($"{nameof(Input)}.{nameof(Input.Username)}", "Username may not be empty.");
                    await LoadAsync(user);
                    return Page();
                }
                if (Input.Username != username)
                {
                    var setUsernameResult = await _userManager.SetUserNameAsync(user, Input.Username);
                    if (!setUsernameResult.Succeeded)
                    {
                        foreach (var error in setUsernameResult.Errors)
                            ModelState.AddModelError($"{nameof(Input)}.{ nameof(Input.Username)}", error.Description);
                        StatusMessage = "There was a problem trying to set username";
                        await LoadAsync(user);
                        return Page();
                    }
                }
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            if (Input.DisplayName != user.DisplayName)
            {
                user.DisplayName = Input.DisplayName;
                var setDisplayNameResult = await _userManager.UpdateAsync(user);
                if (!setDisplayNameResult.Succeeded)
                {
                    StatusMessage = "There was an error trying to update your display name.";
                    foreach (var error in setDisplayNameResult.Errors)
                        ModelState.AddModelError(null, error.Description);
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
