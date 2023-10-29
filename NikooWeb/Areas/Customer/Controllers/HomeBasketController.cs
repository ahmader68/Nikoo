using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nikoo.DataAccess.Repository.IRepository;
using Nikoo.Models.ViewModels;
using Nikoo.Models;
using Nikoo.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.CodeAnalysis;

namespace NikooWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = SD.Role_Customer)]
    public class HomeBasketController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        public HomeBasketController(IUnitOfWork db, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = db;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            string userId = _userManager.GetUserId(HttpContext.User);
            List<Basket> baskets = _unitOfWork.Basket.GetAll().Where(x => x.UserId.Equals(userId)).ToList();
            foreach (Basket basket in baskets)
            {
                basket.TotalFactor = _unitOfWork.Basket.CalculateBasketTotal(basket.Id);
            }
            return View(baskets);
        }
        public IActionResult BasketDetails(int? id)
        {
            BasketViewModel basketVM = new BasketViewModel();

            if (id == null || id == 0)
            {
                // Create
                return View(basketVM);
            }
            else
            {
                // Update
                basketVM.Basket = _unitOfWork.Basket.Get(x => x.Id == id);
                basketVM.Basket.TotalFactor = _unitOfWork.Basket.CalculateBasketTotal(id);
                basketVM.BasketItems = _unitOfWork.BasketItem
                    .GetAll(includeProperties: "Product").Where(x => x.BasketId == id).ToList();
                basketVM.PostTypes = Enum.GetValues(typeof(PostType)).Cast<PostType>().Select(v => new SelectListItem
                {
                    Text = v.ToString(),
                    Value = ((int)v).ToString()
                }).ToList();

                return View(basketVM);
            }

        }

        [HttpPost]
        public IActionResult BasketDetails(Basket Basket)
        {
            if (Basket == null)
            {
                TempData["success"] = "Basket Is Null";
                return RedirectToAction("Index");
            }
            
            IEnumerable<BasketItem> basketItems = _unitOfWork.BasketItem.GetAll(includeProperties: "Product")
                .Where(x => x.BasketId == Basket.Id);
            
            foreach (BasketItem item in basketItems)
            {
                bool HasEnoughProd = _unitOfWork.Basket.CheckStockAvailability(item.ProductId, item.Quantity);

                if (!HasEnoughProd)
                {
                    BasketViewModel basketVM = new BasketViewModel();
                    basketVM.Basket = _unitOfWork.Basket.Get(x => x.Id == Basket.Id);
                    basketVM.Basket.TotalFactor = _unitOfWork.Basket.CalculateBasketTotal(Basket.Id);
                    basketVM.BasketItems = _unitOfWork.BasketItem
                        .GetAll(includeProperties: "Product").Where(x => x.BasketId == Basket.Id).ToList();
                    basketVM.PostTypes = Enum.GetValues(typeof(PostType)).Cast<PostType>().Select(v => new SelectListItem
                    {
                        Text = v.ToString(),
                        Value = ((int)v).ToString()
                    }).ToList();
                    TempData["error"] = $"{item.Product.Title} is not enough exist in store";
                    return RedirectToAction("BasketDetails", basketVM);
                }
            }

            Basket.IsPaid = true;
            Basket.UserId = _userManager.GetUserId(User);
            _unitOfWork.Basket.Update(Basket);
            _unitOfWork.Save();


            return RedirectToAction("Pay", Basket);
        }
        public IActionResult Pay(Basket Basket)
        {
            IEnumerable<BasketItem> basketItems = _unitOfWork.BasketItem.GetAll(includeProperties: "Product")
                .Where(x => x.BasketId == Basket.Id);

            foreach (BasketItem item in basketItems)
            {
                item.Product.SellCount = item.Quantity;
                item.Product.StoreCapacity = item.Product.StoreCapacity - item.Quantity;

                _unitOfWork.Product.Update(item.Product);
                _unitOfWork.Save();
            }

            Payment payment = new Payment
            {
                BasketId = Basket.Id,
                TotalPaid = Basket.FinalFactor,
                IsPaid = true,
                UserId = _userManager.GetUserId(User),
                TrackingCode = new Random(Basket.Id).Next(10000,99999).ToString()
            };

            _unitOfWork.Payment.Add(payment);
            _unitOfWork.Save();

            BasketViewModel basketViewModel = new BasketViewModel
            {
                Basket = Basket,
                Payment = payment
            };

            return View(basketViewModel);
        }

        public IActionResult Delete(int? id)
        {
            Basket? BasketToBeDeleted = _unitOfWork.Basket.Get(x => x.Id == id);

            if (BasketToBeDeleted == null)
            {
                return NotFound();
            }

            _unitOfWork.Basket.Remove(BasketToBeDeleted);
            _unitOfWork.Save();
            TempData["success"] = "Deleted Successfully";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult CheckDiscount(int totalPrice, string discountCode)
        {
            JsonResultMessage message = new JsonResultMessage();
            Discount discount = _unitOfWork.Discount.Get(x => x.DiscountCode == discountCode);
            int TotalPrice = 0;
            if (discount != null)
            {
                if (discount.IsPercent == 0)
                {
                    int discountValue = 0;
                    discountValue = (totalPrice * discount.Value) / 100;
                    TotalPrice = totalPrice - discountValue;
                }
                else if (discount.IsPercent == 1)
                {
                    TotalPrice = totalPrice - discount.Value;
                }
                else
                {
                    message = new JsonResultMessage
                    {
                        FinalPrice = totalPrice.ToString("c0"),
                        Message = "Discount Code is Worong !",
                        DiscountId = discount.Id
                    };
                    return new JsonResult(Ok(message));
                }

                message = new JsonResultMessage
                {
                    FinalPrice = TotalPrice.ToString(),
                    Message = "Discount is done !",
                    DiscountId = discount.Id
                };

                return new JsonResult(Ok(message));
            }
            else
            {
                message = new JsonResultMessage
                {
                    FinalPrice = totalPrice.ToString(),
                    Message = "Discount Code is Wrong !",
                };

                return new JsonResult(Ok(message));
            }
        }
    }
}
