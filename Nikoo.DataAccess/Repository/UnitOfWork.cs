using Nikoo.DataAccess.Data;
using Nikoo.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nikoo.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }
        public IBasketRepository Basket { get; private set; }
        public IBasketItemRepository BasketItem { get; private set; }
        public IDiscountRepository Discount { get; private set; }
        public IPaymentRepository Payment { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(db);
            Product = new ProductRepository(db);
            Basket = new BasketRepository(db);
            BasketItem = new BasketItemRepository(db);
            Discount = new DiscountRepository(db);
            Payment = new PaymentRepository(db);
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
