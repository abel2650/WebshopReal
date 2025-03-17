using ImaginaryShop.Model;
using ImaginaryShop.Model.Repos;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace ImaginaryShop.Pages
{
    public class ProductsModel : PageModel
    {
        private IEnumerable<Product> products;
        public IEnumerable<Product> Products { get; private set; }
        public string SearchTerm { get; set; } // Tilføjet søgeterm

        public void OnGet(string searchTerm)
        {
            SearchTerm = searchTerm;
            ProductRepository r = new ProductRepository("Data Source=mssql16.unoeuro.com,1433;Initial Catalog=mathiasabel_dk_db_abel;User ID=mathiasabel_dk;Password=Hnmxry4ftGBFzeadDwgp;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

            string currency = Request.Cookies.TryGetValue("currency", out string currencyValue) ? currencyValue : null;
            products = r.GetAllProducts(currency);

            // Filtrér produkterne baseret på søgning
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                Products = products.Where(p => p.ProductName.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                Products = products;
            }
        }
    }
}
