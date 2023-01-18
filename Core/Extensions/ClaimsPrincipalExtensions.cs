using Core.DataAccess.Connections;
using Core.Utilities.Business;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static List<string> Claims(this ClaimsPrincipal claimsPrincipal, string claimType)
        {
            return  claimsPrincipal?.FindAll(claimType)?.Select(x=>x.Value).ToList();
        }


        public static int GetUserID(this ClaimsPrincipal claimsPrincipal)
        {
            int id = 0;
            string strid = claimsPrincipal?.FindAll("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Select(x => x.Value).FirstOrDefault();
                int.TryParse(strid, out id);
            return id;

        }


        /// <summary>
        /// Rollleri döndürür
        /// </summary>
        /// <param name="claimsPrincipal"></param>
        /// <returns></returns>
        public static List<string> ClaimRoles(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal?.Claims(ClaimTypes.Role);
        }

        public static bool IsAdmin(this ClaimsPrincipal claimsPrincipal)
        {
            return (bool)(claimsPrincipal?.Claims(ClaimTypes.Role).Contains("Admin"));
        }


        /// <summary>
        /// Yetki Var Mı?
        /// </summary>
        /// <param name="claimsPrincipal">Kullanıcının rolleri (Tokendan geliyor.)</param>
        /// <param name="roles"> ',' seperatörü ile ayrılmış roller</param>
        /// <returns>Rol varsa true yoksa false döner.</returns>
        public static bool HasRoles(this ClaimsPrincipal claimsPrincipal, string roles)
        {
            // Admin ise yetkilidir.
            if ((bool)(claimsPrincipal?.Claims(ClaimTypes.Role).Contains("Admin")))
                return true;
            
            string[] roleArray=roles.Split(',');
            foreach (string role in roleArray)
            {
                if ((bool)(claimsPrincipal?.Claims(ClaimTypes.Role).Contains(role)))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// ilgili servise ait herhangi bir yetkisi var mı?
        /// Örneğin kullanıcının sadece Product.Get yetkisi olsun. Bu durumda Product butonuna erişim sağlayabilir (içinde sadece Get metodlarını görebilecek.)
        /// </summary>
        /// <param name="claimsPrincipal">Kullanıcının rolleri (Tokendan geliyor.)</param>
        /// <param name="roles"> ',' seperatörü ile ayrılmış roller</param>
        /// <returns>Rol varsa true yoksa false döner.</returns>
        public static bool HasAccess(this ClaimsPrincipal claimsPrincipal, string roles)
        {
            // Admin ise yetkilidir.
            if ((bool)(claimsPrincipal?.Claims(ClaimTypes.Role).Contains("Admin")))
                return true;

            string[] roleArray = roles.Split(',');
            foreach (string role in roleArray)
            {
                if ((bool)(claimsPrincipal?.Claims(ClaimTypes.Role).Any(r=>r.StartsWith(role))))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
