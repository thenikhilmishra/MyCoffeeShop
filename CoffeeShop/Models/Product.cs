using System.ComponentModel.DataAnnotations;

public class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Product name is required."), StringLength(100), Display(Name = "Product Name")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Product detail is required."), StringLength(1000), Display(Name = "Detail")]
    public string Detail { get; set; }

    [Range(0.01, 10000, ErrorMessage = "Price must be between 0.01 and 10,000."), Display(Name = "Price")]
    public decimal Price { get; set; }

    public bool IsTrendingProduct { get; set; }

    public string? ImageUrl { get; set; }
}