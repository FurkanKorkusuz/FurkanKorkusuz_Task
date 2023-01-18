
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Concrete
{
    [Table("Users_Permissions")]
    public class UserPermission
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int PermissionID { get; set; }


    }
}
