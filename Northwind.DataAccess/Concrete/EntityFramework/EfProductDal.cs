using Core.DataAccess.EntityFramework;
using Northwind.Entities.Concrete;
using Northwind.DataAccess.Concrete.EntityFramework.Contexts;
using Northwind.DataAccess.Abstract;
using Northwind.Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using Northwind.DataAccess.Settings;

namespace Northwind.DataAccess.Concrete.EntityFramework
{
    public class EfProductDal : EfEntityRepositoryBase<Product, NorthwindContext>, IProductDal
    {
       
        public async Task<List<ProductWithCategoryDto>> GetProductsWithName()
        {
            using (var context = new NorthwindContext())
            {
                var result = from products in context.Products
                             join categories in context.Categories
                             on products.CategoryId equals categories.CategoryId
                             select new ProductWithCategoryDto { ProductId = products.ProductId, ProductName = products.ProductName, CategoryId = products.CategoryId, CategoryName = categories.CategoryName };
                return await result.ToListAsync();
            }
        }
    }
}