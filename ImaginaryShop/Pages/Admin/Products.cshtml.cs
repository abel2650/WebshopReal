using ImaginaryShop.Model;
using ImaginaryShop.Model.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Diagnostics;

namespace ImaginaryShop.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class ProductsModel : PageModel
    {
        private readonly ProductRepository _productRepository;
        private readonly string _defaultCurrency = "DKK";

        public ProductsModel()
        {
            _productRepository = new ProductRepository("Data Source=mssql16.unoeuro.com,1433;Initial Catalog=mathiasabel_dk_db_abel;User ID=mathiasabel_dk;Password=Hnmxry4ftGBFzeadDwgp;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }

        [BindProperty]
        public Product EditingProduct { get; set; } = new Product();

        public IEnumerable<Product> Products { get; private set; }

        private string GetCurrentCurrency()
        {
            return Request.Cookies.TryGetValue("currency", out string currencyValue) ? currencyValue : _defaultCurrency;
        }

        public IActionResult OnGet(int? id)
        {
            string currency = GetCurrentCurrency();
            Products = _productRepository.GetAllProducts(currency);

            if (id.HasValue)
            {
                var product = _productRepository.GetProductById(id.Value, currency);
                if (product != null)
                {
                    EditingProduct = product;
                }
            }

            ModelState.Remove("Currency"); // Fjern eventuelle currency valideringsfejl
            return Page();
        }

        public IActionResult OnPost()
        {
            ModelState.Remove("Currency"); // Fjern currency fra ModelState validering

            if (!ModelState.IsValid)
            {
                Products = _productRepository.GetAllProducts(GetCurrentCurrency());
                return Page();
            }

            try
            {
                EditingProduct.Currency = GetCurrentCurrency();
                EditingProduct.UpdatedAt = DateTime.Now;
                
                if (EditingProduct.ProductID == 0)
                {
                    EditingProduct.CreatedAt = DateTime.Now;
                    _productRepository.AddProduct(EditingProduct);
                }
                else
                {
                    var existingProduct = _productRepository.GetProductById(EditingProduct.ProductID, GetCurrentCurrency());
                    if (existingProduct != null)
                    {
                        EditingProduct.CreatedAt = existingProduct.CreatedAt;
                        _productRepository.UpdateProduct(EditingProduct);
                    }
                }

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Der opstod en fejl ved gem af produktet. Prøv igen.");
                Products = _productRepository.GetAllProducts(GetCurrentCurrency());
                return Page();
            }
        }

        public IActionResult OnPostDelete(int id)
        {
            try
            {
                _productRepository.DeleteProduct(id);
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Der opstod en fejl ved sletning af produktet. Prøv igen.");
                Products = _productRepository.GetAllProducts(GetCurrentCurrency());
                return Page();
            }
        }
    }
} 