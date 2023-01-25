using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Concrete
{
    [Table("Categories")]
    public class Category : BaseEntity
    {
        public int? ParentID { get; set; }
        public string CategoryName { get; set; }
    }

}
