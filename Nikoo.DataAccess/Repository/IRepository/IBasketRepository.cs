using Microsoft.EntityFrameworkCore;
using Nikoo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nikoo.DataAccess.Repository.IRepository
{
    public interface IBasketRepository : IRepository<Basket>
    {
        void Update(Basket obj);
        public int CalculateBasketTotal(int? basketId);
        public bool CheckStockAvailability(int productId, int requestedQuantity);
    }
}
