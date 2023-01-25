using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Entities.Abstract;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Types;

namespace Core.Entities.Concrete
{

    [Table("Products")]
    public class Product : BaseEntity
    {
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public int  Stock { get; set; }
        public string UrlName { get; set; }
        public int CategoryID { get; set; }

    }

    public class ProductFilter : BaseFilter
    {

        public int? ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }

        public int Deleted { get { return 0; } }
        public int Visible { get { return 1; } }
    }
}