using Core.DataAccess.Dapper;
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
    [Table("Users")]
    public class User : BaseEntity
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public bool Status { get; set; }
        public Byte[] PasswordSalt { get; set; }

        public Byte[] PasswordHash { get; set; }
    }
    public class UserFilter : BaseFilter
    {
        public int? ID { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public bool? Status { get; set; }
    }
}
