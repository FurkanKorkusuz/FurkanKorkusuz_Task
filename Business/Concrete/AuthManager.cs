using Business.Abstract;
using Core.BaseResults;
using Core.Entities.Concrete;
using Core.Entities.DTOs;
using Core.Utilities.IoC;
using Core.Utilities.Messages;
using Core.Utilities.Security.Authentication.Utils;
using Core.Utilities.Security.Hashing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
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
        public BaseResult Login(UserForLoginDto userForLoginDto)
        {

            // Burada isteğe bağlı olarak şifre hatalı mesajı da verilebilir (bu durumda email kesin kayıtlıdır), kullanıcı adı ya da şifre hatalı da  denilebilir (bu durumda email kayıtlı mıdır bilinemez).

            // Kullanıcı var mı
            var usertoCheck = _userService.GetEmailorUserNameForLogin(userForLoginDto.EmailOrUserName);
            if (usertoCheck == null)
            {
                return new BaseResult {Error = AuthenticationMessage.UserNotFound };
            }

            // Şifre eşleşiyor mu
            if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Password, usertoCheck.PasswordHash, usertoCheck.PasswordSalt))
            {               
                return new EntityResult<User> {Error= AuthenticationMessage.PasswordError };
            }

            var permissions = _userService.GetPermissionsByUserID(usertoCheck.ID);
            return _tokenHelper.CreateToken(usertoCheck, permissions);
        }

        public BaseResult Register(UserForRegisterDto userForRegister)
        {
            BaseResult result = new BaseResult();

            // Email kontrol.
            if (_userService.CheckEmailIfExist(userForRegister.Email))
            {
                result.Error = AuthenticationMessage.EmailAlreadyExists;
                return result;
            }

            // Password hashlenip kaydedilsin. Buradaki salt 12345 gibi şifrelerin aynı değere hash olmamasını sağlar, bu sayede 12345 bile kolay kolay çözülemez ve karşılığı bulunamaz.
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(userForRegister.Password, out passwordHash, out passwordSalt);
            var user = new User
            {
                Email = userForRegister.Email,
                UserName = userForRegister.UserName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Status = true

            };
            user = _userService.Add(user);
            string token = _tokenHelper.CreateToken(user, new List<Permission>()).Entity.Token;

            return new BaseResult { Data = new {Token= token  } };
        }

    }
}
