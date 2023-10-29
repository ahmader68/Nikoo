using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nikoo.DataAccess.Repository.IRepository;
using Nikoo.Models.ViewModels;
using Nikoo.Models;
using Nikoo.Utility;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text.Json.Nodes;
using Newtonsoft.Json;

namespace NikooWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class BasketController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public BasketController(IUnitOfWork db)
        {
            _unitOfWork = db;

        }
        public IActionResult Index()
        {
            List<Basket> baskets = _unitOfWork.Basket.GetAll().ToList();

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
                    .GetAll(includeProperties:"Product").Where(x => x.BasketId == id).ToList();
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
            if (ModelState.IsValid)
            {
                if (Basket.Id > 0)
                {
                    _unitOfWork.Basket.Update(Basket);
                }
                else
                {
                    _unitOfWork.Basket.Add(Basket);
                }
                _unitOfWork.Save();
                TempData["success"] = "Created Successfully";
                return RedirectToAction("Index");
            }
            return View(Basket);
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
        public JsonResult CheckDiscount(int totalPrice,string discountCode)
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
                else if(discount.IsPercent == 1)
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
                    TempData["success"] = "Discount Code is Worong !";
                    return new JsonResult(Ok(message));
                }

                message = new JsonResultMessage
                {
                    FinalPrice = TotalPrice.ToString(),
                    Message = "Discount is done !",
                    DiscountId = discount.Id
                };
                TempData["success"] = "Discount is OK !";
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
