using ImaginaryShop.Model;
using ImaginaryShop.Model.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace ImaginaryShop.Pages
{
    public class ShoppingBasketModel : ShoppingPageModel
    {
        public ShoppingBasket Basket { get; set; } = new();

        public IActionResult OnGet()
        {
            Basket = GetShoppingBasket();
            return Page();
        }

        [ValidateAntiForgeryToken]
        public IActionResult OnPostUpdateQuantity(int productId, string action)
        {
            Basket = GetShoppingBasket();
            var item = Basket.Products.FirstOrDefault(p => p.Product.ProductID == productId);
            
            if (item != null)
            {
                if (action == "increase")
                {
                    item.Quantity++;
                }
                else if (action == "decrease" && item.Quantity > 1)
                {
                    item.Quantity--;
                }
                SaveBasket(Basket);
            }
            
            return new JsonResult(new { success = true });
        }

        [ValidateAntiForgeryToken]
        public IActionResult OnPostRemoveItem(int productId)
        {
            Basket = GetShoppingBasket();
            var item = Basket.Products.FirstOrDefault(p => p.Product.ProductID == productId);
            
            if (item != null)
            {
                Basket.Products.Remove(item);
                SaveBasket(Basket);
            }
            
            return new JsonResult(new { success = true });
        }

        public IActionResult OnPostQuickAdd(int productId)
        {
            // Henter brugerens indkøbskurv fra sessionen
            Basket = GetShoppingBasket();

            // Hvis brugeren er administrator, forbydes adgang til at tilføje produkter
            if (User.IsInRole("Admin"))
            {
                return Forbid(); // Returnerer en HTTP 403 Forbidden
            }

            // Opretter et repository-objekt for at hente produktdata fra databasen
            ProductRepository r = new ProductRepository("Data Source=mssql16.unoeuro.com,1433;Initial Catalog=mathiasabel_dk_db_abel;User ID=mathiasabel_dk;Password=Hnmxry4ftGBFzeadDwgp;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

            // TODO: Implementer tjek af lagerbeholdning, før produktet tilføjes
            Product productToBeAdded = r.GetProductById(productId, Currency);

            if (productToBeAdded != null)
            {
                // Produktet findes i databasen og tilføjes til indkøbskurven med en mængde på 1
                Basket.AddProduct(productToBeAdded, 1);
            }

            // Gemmer den opdaterede indkøbskurv i sessionen
            SaveBasket(Basket);

            // Returnerer en JSON-respons med den opdaterede totalpris og antal varer i kurven
            return new JsonResult(new
            {
                total = Basket.GetTotal(),
                itemCount = Basket.GetQuantity()
            });
        }
    }
}
