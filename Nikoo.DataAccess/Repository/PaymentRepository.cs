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
    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        private readonly ApplicationDbContext _db;
        public PaymentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Payment obj)
        {
            _db.Update<Payment>(obj);
        }
    }
}
