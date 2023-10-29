using Nikoo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nikoo.DataAccess.Repository.IRepository
{
    public interface IBasketItemRepository : IRepository<BasketItem>
    {
        void Update(BasketItem obj);
    }
}
