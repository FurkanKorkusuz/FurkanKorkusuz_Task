using Business.Abstract;
using Core.BaseResults;
using Core.DataAccess.Abstract;
using Core.Entities.Concrete;
using Core.Utilities.Business;
using DataAccess.Abstract;
using DataAccess.Concrete.Dapper;
using DataAccess.Concrete.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class UserManager : BaseEntityManager<User, UserFilter> ,IUserService
    {
        IUserDal _userDal;
        public UserManager(IUserDal userDal) : base(userDal)
        {
            _userDal = userDal;
        }

        public bool CheckEmailIfExist(string email)
        {
            return _userDal.CheckEmailIfExist(email);
        }

        public User GetEmailorUserNameForLogin(string emailOrUserName)
        {

          return  _userDal.GetEmailorUserNameForLogin(emailOrUserName);
        }

        public List<Permission> GetPermissionsByUserID(int userID)
        {
            return _userDal.GetPermissionsByUserID(userID);
        }
    }
}
