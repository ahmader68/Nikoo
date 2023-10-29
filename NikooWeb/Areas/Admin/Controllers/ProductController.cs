using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using Nikoo.DataAccess.Repository.IRepository;
using Nikoo.Models;
using Nikoo.Models.ViewModels;
using Nikoo.Utility;

namespace NikooWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork db, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = db;
            _webHostEnvironment = webHostEnvironment;

        }
        public IActionResult Index()
        {
            List<Product> products = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
            
            return View(products);
        }
        public IActionResult Upsert(int? id)
        {
            ProductViewModel productModle = new()
            {
                Product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll().Select(
                    u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    })
            };
            if (id == null || id == 0)
            {
                // Create
                return View(productModle);
            }
            else
            {
                // Update
                productModle.Product = _unitOfWork.Product.Get(x => x.Id == id);
                return View(productModle);
            }

        }

        [HttpPost]
        public IActionResult Upsert(ProductViewModel ProductVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string rootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(rootPath, @"images\product");

                    if (!string.IsNullOrEmpty(ProductVM.Product.ImageUrl))
                    {
                        // Delete Old image
                        var oldImagePath = Path.Combine(rootPath, ProductVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using ( var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    ProductVM.Product.ImageUrl = @"\images\product\" + fileName;

                    ViewBag.test = fileName;
                }
                if (ProductVM.Product.Id > 0)
                {
                    _unitOfWork.Product.Update(ProductVM.Product);
                }
                else
                {
                    _unitOfWork.Product.Add(ProductVM.Product);
                }
                _unitOfWork.Save();
                TempData["success"] = "Created Successfully";
                return RedirectToAction("Index");
            }
            else
            {
                ProductVM.CategoryList = _unitOfWork.Category.GetAll().Select(
                        u => new SelectListItem
                        {
                            Text = u.Name,
                            Value = u.Id.ToString()
                        });
                return View(ProductVM);
            }
        }

        public IActionResult Delete(int? id)
        {
            Product? ProductToBeDeleted = _unitOfWork.Product.Get(x => x.Id == id);

            if (ProductToBeDeleted == null)
            {
                return NotFound();
            }

            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, ProductToBeDeleted.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Product.Remove(ProductToBeDeleted);
            _unitOfWork.Save();
            TempData["success"] = "Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
