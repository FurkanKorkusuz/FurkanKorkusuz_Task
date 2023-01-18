using System.ComponentModel.DataAnnotations.Schema;
using Core.Entities.Abstract;
using Dapper.Contrib.Extensions;

namespace Core.Entities.Concrete
{

    public class SuppliedProduct
    {

        public int ProductID { get; set; }
        public int CombinationID { get; set; }
        public int SuppliedCount { get; set; }
    }

    public class SuppliedProductFilter : BaseFilter
    {

        public int? ProductID { get; set; }
        public int? CombinationID { get; set; }
        public int? SuppliedCount { get; set; }

        [Computed]
        public int SupplierID { get; set; }
    }
}