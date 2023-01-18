using Core.DataAccess.Dapper;
using Core.DataAccess.Repositories;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.Dapper
{
    public class DpUserDal : RepositoryBase<User, UserFilter>, IUserDal
    {
        public bool CheckEmailIfExist(string email)
        {
            string sql = @"
                IF EXISTS (SELECT ID FROM Users WHERE Email = @Email) 
                BEGIN
                   SELECT 1
                END
                ELSE
                BEGIN
                    SELECT 0
                END                
            ";
            return DapperUtil.QueryFirstOrDefault<bool>(sql, new { Email = email });
        }

        public User GetEmailorUserNameForLogin(string emailOrUserName)
        {
            string sql = @"
                SELECT	*
		        FROM    Users 
                WHERE UserName = @Value OR Email = @Value
            ";
            return DapperUtil.QueryFirstOrDefault<User>(sql, new { Value = emailOrUserName });
        }

        public List<Permission> GetPermissionsByUserID(int userID)
        {
            string sql = @"
                SELECT p.*
                FROM Users_Permissions up
                JOIN Permissions p ON p.ID = up.PermissionID
                WHERE up.UserID = @UserID
            ";
            return DapperUtil.Query<Permission>(sql, new { UserID = userID }).ToList();
        }
    }
}
