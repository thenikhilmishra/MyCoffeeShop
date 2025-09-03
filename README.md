# CoffeeShop

A modern web application for a coffee shop, built with ASP.NET Core Razor Pages (.NET 8).  
This project demonstrates secure user authentication, product browsing, shopping cart functionality, and a styled contact form—all with a clean, responsive UI.

## Features

- **User Authentication:** Register, login, and manage accounts using ASP.NET Core Identity.
- **Product Catalog:** Browse and shop for coffee products.
- **Shopping Cart:** Add, view, and manage items in your cart.
- **Contact Us:** Submit inquiries via a styled contact form.
- **Responsive Design:** Consistent, modern look across all pages, including authentication and contact forms.
- **Entity Framework Core:** Data access with support for both SQL Server and SQLite.

## Technologies Used

- .NET 8 / ASP.NET Core Razor Pages
- Entity Framework Core (SQL Server & SQLite)
- ASP.NET Core Identity
- Bootstrap 5 (for UI styling)
- Custom CSS for enhanced page layouts

## Getting Started

1. **Clone the repository:**

The app will be available at `https://localhost:5001` (or as configured).

## Project Structure

- `Pages/` - Main Razor Pages (Home, Contact, etc.)
- `Areas/Identity/` - Authentication and user management
- `Views/Shared/_Layout.cshtml` - Main layout with navigation/menu bar
- `wwwroot/` - Static files (CSS, images, JS)

## Customization

- **Styling:**  
The login, register, and contact pages share a consistent, modern style.  
Update background images or CSS in each page as desired.

- **Navigation:** 
The menu bar is defined in `_Layout.cshtml` and is visible on all main pages.

## License

This project is for educational and demonstration purposes.

---

*Built with ❤️ using ASP.NET Core Razor Pages.*
