using CoffeeShop.Data;
using CoffeeShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoffeeShop.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class EditProductModel : PageModel
    {
        private readonly CoffeeShopDbContext _context;
        private readonly IWebHostEnvironment _env;

        public EditProductModel(CoffeeShopDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [BindProperty]
        public Product Product { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Product = await _context.Products.FindAsync(id);
            if (Product == null)
            {
                StatusMessage = "Product not found.";
                return RedirectToPage("ManageProducts");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Log all model errors for debugging
            StatusMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));

            if (!ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(StatusMessage))
                    StatusMessage = "Please correct the errors and try again.";
                return Page();
            }

            var productInDb = await _context.Products.FindAsync(Product.Id);
            if (productInDb == null)
            {
                StatusMessage = "Product not found.";
                return RedirectToPage("ManageProducts");
            }

            productInDb.Name = Product.Name;
            productInDb.Price = Product.Price;
            productInDb.Detail = Product.Detail;
            productInDb.IsTrendingProduct = Product.IsTrendingProduct;

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = Path.GetFileName(ImageFile.FileName);
                var filePath = Path.Combine(_env.WebRootPath, "images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                productInDb.ImageUrl = "/images/" + fileName;
            }

            await _context.SaveChangesAsync();
            StatusMessage = "Product updated successfully!";
            return RedirectToPage("ManageProducts");
        }
    }
}