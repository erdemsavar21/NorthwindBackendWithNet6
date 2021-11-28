using Microsoft.AspNetCore.Mvc;
using Northwind.WebApp.Clients;
using Northwind.WebApp.Models;

namespace Northwind.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProductClient _productClient;
        public HomeController(ProductClient productClient)
        {
            _productClient = productClient;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _productClient.GetProducts();
            if (result.IsSuccess)
            {
                ProductListViewModel model = new ProductListViewModel {Products= result.Data};

                return View(model);

            }
                
            return View();
        }
        
        public async Task<IActionResult> Index1()
        {
            var result = await _productClient.GetProducts();
            if (result.IsSuccess)
            {
                ProductListViewModel model = new ProductListViewModel {Products= result.Data};

                return View(model);

            }
                
            return View();
        }

    }
}