using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ImaginaryShop.Model;
using System.ComponentModel.DataAnnotations;

namespace ImaginaryShop.Pages
{
    public class PaymentModel : ShoppingPageModel
    {
        public ShoppingBasket Basket { get; set; }

        [BindProperty]
        public PaymentFormModel PaymentForm { get; set; }

        public void OnGet()
        {
            Basket = GetShoppingBasket();
            if (Basket?.Products?.Any() != true)
            {
                Response.Redirect("/ShoppingBasket");
            }
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Here you would typically process the payment
            // For now, we'll just redirect to confirmation
            return RedirectToPage("/OrderConfirmation");
        }
    }

    public class PaymentFormModel
    {
        [Required(ErrorMessage = "Navn på kort er påkrævet")]
        public string CardName { get; set; }

        [Required(ErrorMessage = "Kortnummer er påkrævet")]
        [RegularExpression(@"^\d{16}$", ErrorMessage = "Kortnummer skal være 16 cifre")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "Udløbsdato er påkrævet")]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/([0-9]{2})$", ErrorMessage = "Udløbsdato skal være i formatet MM/ÅÅ")]
        public string ExpDate { get; set; }

        [Required(ErrorMessage = "CVV er påkrævet")]
        [RegularExpression(@"^\d{3}$", ErrorMessage = "CVV skal være 3 cifre")]
        public string CVV { get; set; }
    }
} 