using Core.DataAccess;
using Core.Entities.Concrete;

namespace Northwind.DataAccess.Abstract
{
    public interface IUserDal : IEntityRepository<User>
    {
         Task<List<OperationClaim>> GetClaims(User user);
    }
}