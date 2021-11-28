using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using Northwind.DataAccess.Abstract;
using Northwind.DataAccess.Concrete.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;
using Northwind.DataAccess.Settings;

namespace Northwind.DataAccess.Concrete.EntityFramework
{
    public class EfUserDal : EfEntityRepositoryBase<User, NorthwindContext>, IUserDal
    {
        public async Task<List<OperationClaim>> GetClaims(User user)
        {
            using (var context = new NorthwindContext())
            {
                var result = from operationClaim in context.OperationClaims
                             join userOperationClaim in context.UserOperationClaims
                                 on operationClaim.Id equals userOperationClaim.OperationClaimId
                             where userOperationClaim.UserId == user.Id
                             select new OperationClaim { Id = operationClaim.Id, Name = operationClaim.Name };
                return await result.ToListAsync();
            }
        }
    }
}