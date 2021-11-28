using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.Aspects.Autofac.Performance;
using Core.Aspects.Autofac.Security;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Log4Net;
using Core.CrossCuttingConcerns.Logging.Log4Net.Loggers;
using Core.Utilities.Business;
using Core.Utilities.Results;
using Northwind.Business.Abstract;
using Northwind.Business.Constant;
using Northwind.Business.ValidationRules.FluentValidation;
using Northwind.DataAccess.Abstract;
using Northwind.Entities.Concrete;
using Northwind.Entities.Dtos;


namespace Northwind.Business.Concrete
{
    public class ProductManager : IProductService
    {
        private IProductDal _productDal;
        private LoggerServiceBase _loggerServiceBase;

        public ProductManager(IProductDal productDal)
        {
            _productDal = productDal;
            _loggerServiceBase = (LoggerServiceBase)Activator.CreateInstance(typeof(FileLogger));
        }

        [ValidationAspect(typeof(ProductValidator), Priority = 1)]
        [TransactionScopeAspect]
        [CacheRemoveAspect("IProductService.Get")]
        [LogAspect(typeof(FileLogger))]
        public async Task<IResult> Add(Product product)
        {
            IResult result = BusinessRules.Run(await CheckIfProductNameExists(product.ProductName), CheckIfCategoryNameExists(product.CategoryId)); //Buraya birden fazla is tanimi verilabilir. CheckIfProductNameExists(product.ProductName),Check....
            if (result != null)
            {
                return result;
            }
            await _productDal.Add(product);
            return new SuccessResult(Messages.ProductAdded);
        }

        public async Task<IResult> Delete(Product product)
        {
            await _productDal.Delete(product);
            return new SuccessResult(Messages.ProductDeleted);
        }

        public async Task<IDataResult<Product>> GetById(int productId)
        {
            return new SuccessDataResult<Product>(await _productDal.Get(p => p.ProductId == productId));
        }

        [CacheAspect(1)]
        [SecuredOperationAspect("Admin,User")]
        [PerformanceAspect(1, typeof(FileLogger))]
        [LogAspect(typeof(FileLogger))]
        public async Task<IDataResult<List<Product>>> GetList()
        {
            _loggerServiceBase.Info("Get List Servisi Cagrildi");
            var productList = await _productDal.GetList();
            return new SuccessDataResult<List<Product>>(productList.ToList());
        }

        public async Task<IDataResult<List<Product>>> GetListByCategory(int categoryId)
        {
            var productList = await _productDal.GetList(p => p.CategoryId == categoryId);
            return new SuccessDataResult<List<Product>>(productList.ToList());
        }

        public async Task<IDataResult<List<ProductWithCategoryDto>>> GetProductsWithName()
        {
            var productList = await _productDal.GetProductsWithName();
            return new SuccessDataResult<List<ProductWithCategoryDto>>(productList.ToList());
        }

        public async Task<IResult> Update(Product product)
        {
            await _productDal.Update(product);
            return new SuccessResult(Messages.ProductUpdated);
        }

        private async Task<IResult> CheckIfProductNameExists(string productName)
        {
            var result = await _productDal.GetList(p => p.ProductName == productName);
            if (result.Any())
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }

            return new SuccessResult();
        }

        private IResult CheckIfCategoryNameExists(int categoryId)
        {
            return new SuccessResult();
        }

    }
}