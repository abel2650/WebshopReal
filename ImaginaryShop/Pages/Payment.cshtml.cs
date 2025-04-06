using ImaginaryShop.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ImaginaryShop.Pages
{
    public class PaymentModel : ShoppingPageModel
    {
        public ShoppingBasket Basket { get; set; }

        public IActionResult OnGet()
        {
            Basket = GetShoppingBasket();
            return Page();
        }
    }
} 