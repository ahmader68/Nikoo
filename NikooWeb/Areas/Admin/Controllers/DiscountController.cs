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
using Microsoft.AspNetCore.Hosting;
using System;

namespace NikooWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class DiscountController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public DiscountController(IUnitOfWork db)
        {
            _unitOfWork = db;

        }
        public IActionResult Index()
        {
            List<Discount> discounts = _unitOfWork.Discount.GetAll().ToList();

            return View(discounts);
        }
        public IActionResult Upsert(int? id)
        {
            IEnumerable<SelectListItem> discountList = Enum.GetValues(typeof(DiscountType)).Cast<DiscountType>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            });

            DiscountViewModel discountModle = new()
            {
                Discount = new Discount(),
                DiscountList = Enum.GetValues(typeof(DiscountType)).Cast<DiscountType>().Select(v => new SelectListItem
                {
                    Text = v.ToString(),
                    Value = ((int)v).ToString()
                }).ToList()
        };
            if (id == null || id == 0)
            {
                // Create
                return View(discountModle);
            }
            else
            {
                // Update
                discountModle.Discount = _unitOfWork.Discount.Get(x => x.Id == id);
                return View(discountModle);
            }

        }

        [HttpPost]
        public IActionResult Upsert(DiscountViewModel DiscountVM)
        {
            if (ModelState.IsValid)
            {
                if (DiscountVM.Discount.Id > 0)
                {
                    _unitOfWork.Discount.Update(DiscountVM.Discount);
                }
                else
                {
                    _unitOfWork.Discount.Add(DiscountVM.Discount);
                }
                _unitOfWork.Save();
                TempData["success"] = "Created Successfully";
                return RedirectToAction("Index");
            }
            else
            {
                DiscountVM.DiscountList = Enum.GetValues(typeof(DiscountType)).Cast<DiscountType>().Select(v => new SelectListItem
                {
                    Text = v.ToString(),
                    Value = ((int)v).ToString()
                });
                return View(DiscountVM);
            }
        }
        public IActionResult Delete(int? id)
        {
            Discount? DiscountToBeDeleted = _unitOfWork.Discount.Get(x => x.Id == id);

            if (DiscountToBeDeleted == null)
            {
                return NotFound();
            }

            _unitOfWork.Discount.Remove(DiscountToBeDeleted);
            _unitOfWork.Save();
            TempData["success"] = "Deleted Successfully";
            return RedirectToAction("Index");
        }

    }
}
