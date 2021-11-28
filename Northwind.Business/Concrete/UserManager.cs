using Core.Entities.Concrete;
using Core.Utilities.Results;
using Northwind.Business.Abstract;
using Northwind.Business.Constant;
using Northwind.DataAccess.Abstract;


namespace Northwind.Business.Concrete
{
    public class UserManager : IUserService
    {
        private IUserDal _userDal;
        public UserManager(IUserDal userDal)
        {
            _userDal = userDal;
        }

        public async Task<IResult> Add(User user)
        {
           await _userDal.Add(user);
           return new SuccessResult();
        }

        public async Task<IDataResult<User>> GetByMail(string email)
        {
            var result = await _userDal.Get(p => p.Email == email);

            if (result == null)
            {
                return new ErrorDataResult<User>("User Not Found");
            }

            return new SuccessDataResult<User>(result);
        }

        public async Task<IDataResult<List<OperationClaim>>> GetClaims(User user)
        {
            return new SuccessDataResult<List<OperationClaim>>(await _userDal.GetClaims(user));
        }
    }
}