using ImaginaryShop.Model;
using ImaginaryShop.Model.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ImaginaryShop.Pages
{
    public class AddAccountModel : PageModel
    {
        private readonly IUserRepository _userRepository;

        [BindProperty]
        public User NewUser { get; set; }

        public AddAccountModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _userRepository.CreateUser(NewUser);
            return RedirectToPage("Index");
        }
    }
}
