using Core.BaseResults;
using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Security.Authentication.Utils
{
    public interface ITokenHelper
    {
        EntityResult<AccessToken> CreateToken(User user, List<Permission> permissions);
    }
}
