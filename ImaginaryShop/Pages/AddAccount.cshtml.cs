using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ImaginaryShop.Model;
using ImaginaryShop.Model.Repos;
using System.Diagnostics;
using Isopoh.Cryptography.Argon2;

namespace ImaginaryShop.Pages
{
    public class AddAccountModel : PageModel
    {
        [BindProperty]
        public User NewUser { get; set; } = new User();

        [BindProperty]
        public string ConfirmPassword { get; set; }

        private readonly IUserRepository _userRepository;

        public AddAccountModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult OnPost()
        {
            // Debugging: Udskriv modtaget brugerdata
            Debug.WriteLine($"[DEBUG] NewUser: {NewUser}");
            Debug.WriteLine($"[DEBUG] Navn: {NewUser?.FullName}");
            Debug.WriteLine($"[DEBUG] Username: {NewUser?.UserName}");
            Debug.WriteLine($"[DEBUG] Email: {NewUser?.Email}");
            Debug.WriteLine($"[DEBUG] Password (Before Hashing): {NewUser?.PasswordHash}");
            Debug.WriteLine($"[DEBUG] Confirm Password: {ConfirmPassword}");

            if (NewUser == null || string.IsNullOrEmpty(NewUser.Email) || string.IsNullOrEmpty(NewUser.PasswordHash))
            {
                Debug.WriteLine("[FEJL] Brugerdata er ikke korrekt modtaget!");
                ModelState.AddModelError(string.Empty, "Alle felter skal udfyldes.");
                return Page();
            }

            if (string.IsNullOrEmpty(ConfirmPassword))
            {
                Debug.WriteLine("[FEJL] Bekræftelsespassword mangler!");
                ModelState.AddModelError(string.Empty, "Du skal bekræfte dit password.");
                return Page();
            }

            if (NewUser.PasswordHash != ConfirmPassword)
            {
                Debug.WriteLine("[FEJL] Passwords matcher ikke!");
                ModelState.AddModelError(string.Empty, "Passwords matcher ikke.");
                return Page();
            }

            try
            {
                // Hash passwordet med Argon2, før det gemmes
                NewUser.PasswordHash = Argon2.Hash(NewUser.PasswordHash);

                // Sæt brugerens rolle til Customer
                NewUser.Role = ImaginaryShop.Model.User.UserRole.Customer;
                
                NewUser.CreatedAt = DateTime.UtcNow;
                _userRepository.AddUser(NewUser);

                Debug.WriteLine($"[DEBUG] Password (After Hashing): {NewUser.PasswordHash}");
                Debug.WriteLine("[SUCCESS] Bruger oprettet!");

                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[FEJL] {ex.Message}");
                ModelState.AddModelError(string.Empty, "Der opstod en fejl ved oprettelse af brugeren. Prøv igen senere.");
                return Page();
            }
        }
    }
}
