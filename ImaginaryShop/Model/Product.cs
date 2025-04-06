using ImaginaryShop.Controllers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImaginaryShop.Model
{
    public class Product
    {
        public int ProductID { get; set; }          // Unik identifikation for produktet
        
        [Required(ErrorMessage = "Produktnavn er påkrævet")]
        public string ProductName { get; set; }     // Navn på produktet
        
        [Required(ErrorMessage = "Beskrivelse er påkrævet")]
        public string Description { get; set; }      // Beskrivelse af produktet
        
        [Required(ErrorMessage = "Pris er påkrævet")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Pris skal være større end 0")]
        public decimal Price { get; set; }           // Pris på produktet
        
        [Required(ErrorMessage = "Lagerbeholdning er påkrævet")]
        [Range(0, int.MaxValue, ErrorMessage = "Lagerbeholdning skal være 0 eller større")]
        public int StockQuantity { get; set; }       // Antal på lager
        
        [Required(ErrorMessage = "Kategori er påkrævet")]
        public string Category { get; set; }          // Kategori for produktet
        
        [Required(ErrorMessage = "Billede URL er påkrævet")]
        public string ImageUrl { get; set; }         // URL til produktbilledet
        
        public DateTime CreatedAt { get; set; }      // Dato for oprettelse
        public DateTime UpdatedAt { get; set; }      // Dato for seneste opdatering

        [NotMapped] // Dette felt gemmes ikke direkte i databasen
        public string Currency { get; set; } = "DKK"; // Default værdi

        public Product() { }
        public Product(int productId, string productName, string description, decimal price, int stockQuantity, string category, string imageUrl, string currency)
        {
            ProductID = productId;
            ProductName = productName;
            Description = description;
            Price = price;
            StockQuantity = stockQuantity;
            Category = category;
            ImageUrl = imageUrl;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            Currency = currency;
        }
    }
}
