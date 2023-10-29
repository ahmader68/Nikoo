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
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Product obj)
        {
            var objFromDb = _db.Products.FirstOrDefault(x => x.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Title = obj.Title;
                objFromDb.Description = obj.Description;
                objFromDb.Price = obj.Price;
                objFromDb.StoreCapacity = obj.StoreCapacity;
                objFromDb.IsImportant = obj.IsImportant;
                objFromDb.IsSuggested = obj.IsSuggested;
                objFromDb.IsActive = obj.IsActive;
                objFromDb.CategoryId = obj.CategoryId;
                objFromDb.SellCount = obj.SellCount;
                if (obj.ImageUrl != null)
                {
                    objFromDb.ImageUrl = obj.ImageUrl;
                }
            }
            //_db.Update<Product>(obj);
        }
    }
}
