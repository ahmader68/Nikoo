using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Nikoo.DataAccess.Repository;
using Nikoo.DataAccess.Repository.IRepository;
using Nikoo.Models;
using Nikoo.Models.ViewModels;
using System.Diagnostics;

namespace NikooWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> products = _unitOfWork.Product.GetAll(includeProperties:"Category");
            return View(products);
        }
        public IActionResult Details(int ProductId)
        {
            Product product = _unitOfWork.Product.Get(x => x.Id == ProductId, includeProperties:"Category");
            return View(product);
        }
        public IActionResult AddToBasket(int ProductId, int Quantity)
        {
            string userId = _userManager.GetUserId(HttpContext.User);
            if (string.IsNullOrEmpty(userId))
            {
                TempData["error"] = "Please Login First !";
                return RedirectToAction("Details", new { ProductId = ProductId } );
            }

            bool HasEnoughProd = _unitOfWork.Basket.CheckStockAvailability(ProductId, Quantity);

            if (!HasEnoughProd)
            {
                TempData["error"] = "Product is NOT Enough !";
                return RedirectToAction("Details", new { ProductId = ProductId });
            }

            Product product = _unitOfWork.Product.Get(x => x.Id == ProductId);
            
            Basket basket = _unitOfWork.Basket.Get(x => x.IsPaid == false && x.UserId == userId);

            if (basket != null)
            {
                BasketItem basketItem = new()
                {
                    ProductId = product.Id,
                    Quantity = Quantity,
                    BasketId = basket.Id,
                    UnitPrice = product.Price,
                };
                _unitOfWork.BasketItem.Add(basketItem);
                _unitOfWork.Save();
                basket.TotalFactor = _unitOfWork.Basket.CalculateBasketTotal(basket.Id);
                _unitOfWork.Basket.Update(basket);
                _unitOfWork.Save();
            }
            else
            {
                Basket newBasket = new()
                {
                    UserId = userId,
                    CreateDate = DateTime.Now,
                    TotalFactor = product.Price,
                    IsPaid = false,
                };
                _unitOfWork.Basket.Add(newBasket);
                _unitOfWork.Save();

                BasketItem basketItem = new()
                {
                    ProductId = product.Id,
                    Quantity = Quantity,
                    BasketId = newBasket.Id,
                    UnitPrice = product.Price,
                };
                _unitOfWork.BasketItem.Add(basketItem);
                _unitOfWork.Save();
            }

            TempData["success"] = "Add To Basket Successfully";
            return RedirectToAction("Details", new { ProductId = ProductId });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}