using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.Jwt;
using Northwind.Business.Abstract;
using Northwind.Business.Constant;
using Northwind.Entities.Dtos;

namespace Northwind.Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private IUserService _userService;
        private ITokenHelper _tokenHelper;
        public AuthManager(IUserService userService, ITokenHelper tokenHelper)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
        }

        public async Task<IDataResult<AccessToken>> CreateAccessToken(User user)
        {
            var claims = await _userService.GetClaims(user);
            var accessToken = _tokenHelper.CreateToken(user, claims.Data);
            return new SuccessDataResult<AccessToken>(accessToken, Messages.AccessTokenCreated);
        }

        public async Task<IDataResult<User>> Login(UserForLoginDto userForLoginDto)
        {
            var userToCheck = await _userService.GetByMail(userForLoginDto.Email);

            if (userToCheck.Data == null || !userToCheck.Success)
            {
                return new ErrorDataResult<User>(Messages.UserNotFound);
            }

            if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Password, userToCheck.Data.PasswordHash, userToCheck.Data.PasswordSalt))
            {
                return new ErrorDataResult<User>(Messages.PasswordError);
            }


            return new SuccessDataResult<User>(userToCheck.Data, Messages.SuccessfulLogin);


        }

        public async Task<IDataResult<User>> Register(UserForRegisterDto userForRegisterDto, string password)
        {
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);
            var user = new User
            {
                Email = userForRegisterDto.Email,
                FirstName = userForRegisterDto.FirstName,
                LastName = userForRegisterDto.LastName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Status = true
            };
            await _userService.Add(user);
            return new SuccessDataResult<User>(user, Messages.UserRegistered);
        }

        public async Task<IResult> UserExists(string email)
        {
            var result = await _userService.GetByMail(email);
            if (result.Success)
            {
                return new ErrorResult(Messages.UserAlreadyExists);
            }

            return new SuccessResult();
        }

    }
}