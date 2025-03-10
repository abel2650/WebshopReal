using ImaginaryShop.Model;
using ImaginaryShop.Model.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ImaginaryShop.Pages
{
    public class ProductsModel : PageModel
    {
        IEnumerable<Product> products;

        public IEnumerable<Product> Products { get => products; set => products = value; }

        public void OnGet()
        {
            ProductRepository r = new ProductRepository("Data Source=mssql16.unoeuro.com,1433;Initial Catalog=mathiasabel_dk_db_abel;User ID=mathiasabel_dk;Password=Hnmxry4ftGBFzeadDwgp;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");


            //Her aflæser vi lige om currency cookie er sat
            string currency = null;
            if (Request.Cookies.TryGetValue("currency", out string currencyValue))
            {
                currency = currencyValue;
            }
            Products = r.GetAllProducts(currency);
        }
    }
}
