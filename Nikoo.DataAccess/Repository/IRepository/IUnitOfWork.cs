using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nikoo.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; } 
        IProductRepository Product { get; }
        IBasketRepository Basket { get; } 
        IBasketItemRepository BasketItem { get; }
        IDiscountRepository Discount { get; }
        IPaymentRepository Payment { get; }
        void Save();
    }
}
