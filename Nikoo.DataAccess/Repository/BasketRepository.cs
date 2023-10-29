using Microsoft.EntityFrameworkCore;
using Nikoo.DataAccess.Data;
using Nikoo.DataAccess.Repository.IRepository;
using Nikoo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Nikoo.DataAccess.Repository
{
    public class BasketRepository : Repository<Basket>, IBasketRepository
    {
        private ApplicationDbContext _db;
        public BasketRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Basket obj)
        {
            _db.Update<Basket>(obj);
        }

        public int CalculateBasketTotal(int? basketId)
        {
            int totalAmount = _db.BasketItem
                .Where(bi => bi.BasketId == basketId)
                .Sum(bi => bi.Product.Price * bi.Quantity);

            return totalAmount;
        }

        public bool CheckStockAvailability(int productId, int requestedQuantity)
        {
            var product = _db.Products.SingleOrDefault(p => p.Id == productId);

            if (product != null && product.StoreCapacity >= requestedQuantity)
            {
                return true; // موجودی کافی است
            }
            else
            {
                return false; // موجودی کافی نیست
            }
        }
    }
}
