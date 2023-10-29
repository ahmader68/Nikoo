using Nikoo.DataAccess.Data;
using Nikoo.DataAccess.Repository.IRepository;
using Nikoo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nikoo.DataAccess.Repository
{
    public class DiscountRepository : Repository<Discount>, IDiscountRepository
    {
        private ApplicationDbContext _db;
        public DiscountRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Discount obj)
        {
            _db.Update<Discount>(obj);
        }
    }
}
