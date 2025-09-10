using CoffeeShop.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShop.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class ManageProductsModel : PageModel
    {
        private readonly CoffeeShopDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ManageProductsModel(CoffeeShopDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [BindProperty]
        public Product Product { get; set; } = new Product { Name = string.Empty, Detail = string.Empty };

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public List<Product> Products { get; set; } = new();

        [TempData]
        public string? StatusMessage { get; set; }

        public async Task OnGetAsync()
        {
            Products = await _context.Products.ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Products = await _context.Products.ToListAsync();

            // Require image for new products
            if (ImageFile == null || ImageFile.Length == 0)
            {
                ModelState.AddModelError("ImageFile", "Product image is required.");
            }

            // Log all model errors for debugging
            StatusMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));

            if (!ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(StatusMessage))
                    StatusMessage = "Please correct the errors and try again.";
                return Page();
            }

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = Path.GetFileName(ImageFile.FileName);
                var filePath = Path.Combine(_env.WebRootPath, "images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                Product.ImageUrl = "/images/" + fileName;
            }

            _context.Products.Add(Product);
            await _context.SaveChangesAsync();

            StatusMessage = "Product added successfully!";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                StatusMessage = "Product deleted successfully!";
            }
            else
            {
                StatusMessage = "Product not found.";
            }
            return RedirectToPage();
        }
    }
}