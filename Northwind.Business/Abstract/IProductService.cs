using Core.Utilities.Results;
using Northwind.Entities.Concrete;
using Northwind.Entities.Dtos;

namespace Northwind.Business.Abstract
{
    public interface IProductService
    {
         Task<IDataResult<List<Product>>> GetList();
         Task<IDataResult<List<ProductWithCategoryDto>>> GetProductsWithName();
         Task<IDataResult<Product>> GetById(int productId);
         Task<IDataResult<List<Product>>> GetListByCategory(int categoryId);
         Task<IResult> Add(Product product);
         Task<IResult> Delete(Product product);
         Task<IResult> Update(Product product);
    }
}