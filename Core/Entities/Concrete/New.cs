using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Entities.Abstract;

namespace Core.Entities.Concrete
{
  
    [Table("News")]
    public class New : IEntity
    {
        [Key]
        public int ID { get; set; }
        public int UserID { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string UrlLink { get; set; }
    }

    public class NewFilter : BaseFilter
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public bool IsActive { get; set; }
        public string CreatedDate { get; set; }
    }
}