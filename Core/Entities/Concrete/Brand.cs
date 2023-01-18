using Core.Entities.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Concrete
{
    /// <summary>
    /// Generic Repository de tablo adı dinamik olsun diye buradaki  [Table("Brands")] Attribute ünden okuyorum.
    /// </summary>
    [Table("Brands")]
    public  class Brand: IEntity
    {
        public string BrandName { get; set; }
    }

    public class BrandFilter : BaseFilter
    {
        public string BrandName { get; set; }
    }
}
