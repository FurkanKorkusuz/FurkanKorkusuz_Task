using Core.BaseResults;
using Core.Entities.Concrete;
using Core.Entities.DTOs;
using Core.Utilities.Security.Authentication.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IAuthService
    {
        BaseResult Login(UserForLoginDto userForLoginDto);
        BaseResult Register(UserForRegisterDto userForLoginDto);
    }
}
