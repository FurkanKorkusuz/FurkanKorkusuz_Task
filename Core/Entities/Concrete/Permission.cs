using Core.Entities;
using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Concrete
{
    [Table("Permissions")]
    public class Permission:BaseEntity<Permission>
    {
        public string PermissionName { get; set; }
    }
    public class PermissionFilter : BaseFilter
    {

        public string PermissionName { get; set; }
    }
}
