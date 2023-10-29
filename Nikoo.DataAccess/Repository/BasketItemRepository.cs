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
    public class BasketItemRepository : Repository<BasketItem>, IBasketItemRepository
    {
        private ApplicationDbContext _db;
        public BasketItemRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(BasketItem obj)
        {
            _db.Update<BasketItem>(obj);
        }
    }
}
