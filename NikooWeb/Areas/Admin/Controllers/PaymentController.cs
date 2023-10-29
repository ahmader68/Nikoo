using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nikoo.DataAccess.Repository.IRepository;
using Nikoo.Models;
using Nikoo.Utility;

namespace NikooWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class PaymentController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public PaymentController(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Payment> payments = _unitOfWork.Payment.GetAll().ToList();
            return View(payments);
        }
    }
}
